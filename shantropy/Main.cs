/* author: lsauer, 2011;
 * site: https://github.com/lsauer/entropy
 * license: MIT license
 * purpose: quickly plotting entropy information and string metrics from files or strings e.g. via a pipe: dir | ent  -b 2.15 -s 
 * 
 * In information theory, entropy is a measure of the uncertainty associated with a random variable. 
 * In this context, the term usually refers to the Shannon entropy, which quantifies the expected value
 * of the information contained in a message, usually in units such as bits. In this context, a 'message'
 * means a specific realization of the random variable. (src: http://en.wikipedia.org/wiki/Entropy_(information_theory))
 * 
 *  For data compression this means that a data stream with high entropy has relatively few repeated patterns, which decreases 
 * compression ratio. A highly compressed stream will have an entropy >5 for base 2
 * 
 * ### PARAMETERS AND USAGE
 * zero-order Markov source: default (pratically identical with Shannon entropy when the log-base is 2)
 * first-order Markov source: -m 1 ..number of linked characters
 * second-order Markov source: -m 2
 * 
 * different base:  "b-ary entropy" can be set with -b <decimal> e.g. shantropy -b 2,15
 * 
 * arbitrary string passing: -s stringdata; Note  -s has to be the last argument!
 * 
 * -w <int> sets width of the plot
 * -h <int> sets height of the plot
 * 
 * output Stdout to text e.g. shantropy .... > myfile.out
 * -o outfilename ...will plot data to a given file and create or append to the file
 * 
 * -e ...plot the efficiency
 * 
 * Files have to be passed first, before any parameters
 * calculate metrics for several files e.g. ent explain.nfo markdownsharp-20100703-v113.7z -b 3,6
 * 
 * Todo: 
 * 	+make and use an console argument hash map or struct params
 *  +code cleanup
 * Note: quite slow at the moment -> fixed; this was due to Readline loop; up to 200x speedup
 * for text files set -b 64, to get meaninful results
 * 
 * Example of a .nfo file:
 * the ordinate(y-axis) shows the entropy and the abscisse (x-axis) the segment position in percent of the file
 * result: the text is highly compressible and structured
	0,50 |
	0,45 |                                ▓
	0,40 |                     ▓▓        ▓▓ ▓▓   ▓
	0,35 | ▓               ▓  ▓▓▓        ▓▓▓▓▓▓▓▓▓    ▓   ▓▓
	0,30 | ▓               ▓  ▓▓▓▓   ▓   ▓▓▓▓▓▓▓▓▓    ▓  ▓▓▓
	0,25 | ▓               ▓  ▓▓▓▓   ▓  ▓▓▓▓▓▓▓▓▓▓    ▓  ▓▓▓   ▓
	0,20 | ▓               ▓  ▓▓▓▓   ▓  ▓▓▓▓▓▓▓▓▓▓▓   ▓  ▓▓▓  ▓▓
	0,15 | ▓ ▓▓     ▓ ▓ ▓ ▓▓▓ ▓▓▓▓  ▓▓ ▓▓▓▓▓▓▓▓▓▓▓▓  ▓▓  ▓▓▓  ▓▓   ▓
	0,10 | ▓ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ▓▓▓▓▓▓▓▓ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ▓▓▓  ▓▓▓▓▓▓▓▓▓
	0,05 | ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  ▓▓▓▓▓▓▓▓▓
	0,00 | ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓
	------------------------------------------------------------------
	     0%        16%        33%        50%        66%        83%
 * 
 * Useful links: http://www.tiem.utk.edu/~mbeals/shannonDI.html
 * 
 * Fork it on github: https://github.com/lsauer/entropy
 * Make sure to take advantage of the message button on github if you have feedback about any of this stuff.
 * 
 * Have fun! In fact don't use this program for anything else (yet)...
 * 
 * */


using System;
using System.IO;
using System.Collections.Generic; //for dictionary
//using Math; //for Math.Round
using System.Diagnostics; // for Debug.Writeline
/*
namespace System
{
	public class Console{
		public static void WriteLine_(String str) { 
			System.Console.WriteLine(str);
		}
	}
}
 */
namespace shantropy
{
	class Entropy
	{
		private static string[] args;
		
		//extract console parameters, e.g. argname = "-s"
		public static string getArg(string argname)
		{
			int index = Array.IndexOf(Entropy.args, argname);
			if( Entropy.args.Length < index+1 || Entropy.args[index+1][0] == '-' ){
				System.Console.WriteLine("parameter value is missing, or value has '-' as first symbol");
				return null;
			}
			if( index != -1 ){
				return Entropy.args[index+1];
			}else
				return null;
		}
		
		/// Main Entry point
		public static void Main (string[] args)
		{
			//add params, for debugging purposes;  Array.Resize would work as well
			List<string> cpargs = new List<string>(args);
			/*cpargs.Add("EXPLOSiON.NFO");
			cpargs.Add("-h");
			cpargs.Add("4");
			args = cpargs.ToArray();
	        */
			//todo: process params
			/*var argmap = new <string, string>();
			foreach(string str in args){
				....
			}*/
			//create internal reference; access parameters with e.g. Entropy.getArg("-s")
			Entropy.args = args;
			
			//Program message
			System.Console.WriteLine();
			string str_introtxt = "| Ent v09/2011 Entropy calculator & string metrics; lsauer - univie |";
			System.Console.WriteLine("‡".PadRight(str_introtxt.Length-1,'=')+'‡');
			System.Console.WriteLine(str_introtxt);
			System.Console.WriteLine("‡".PadRight(str_introtxt.Length-1,'=')+'‡');
			System.Console.WriteLine();
			
				// Without a filename or string passed, display the help
	        if (args.Length == 0 || args[0] == "-h" || args[0] == "?")
	        {
	            System.Console.WriteLine(	"Usage: shantropy [<filename1> <filename2>...first params!] [-o <outfile>] [-h help]\n"+
											"[-e efficiency] [-m 1,2.. 1st,2nd order markov] [-b base-alphabet] \n"+
											"[-w width plot] [-h height plot] [-s <string> as last param!]\n"
				);
	            return;
	        }
			int index_arg_str = Array.IndexOf(args,"-s");
			int index_arg_base = Array.IndexOf(args,"-b");
			int arg_efficiency = (Array.IndexOf(args,"-e") == -1) ? 0 : 1;
			int arg_markov_m1 =  Entropy.getArg("-m") != null ? int.Parse(Entropy.getArg("-m")) : 0;
			float logbase = 256f;	 //holds the base passed to Math.log for the entropy calc; for ASCII...alphabet size is 256
			string file_name = args[0];
			string data = "", con_buffer = "", line_end = "\n"; //holds the string to be processed
			//check logbase param
			 if (index_arg_base != -1)
				logbase = (float) float.Parse( args[index_arg_base+1] );
			
			//File-Mode: try to open the provided file
			if ( index_arg_str == -1 )
			{ 
				//string[] file_names;
				//Array.Copy(args,  args[Array.IndexOf(args,"-s")
				//foreach( string file_name in args)...
				for(int i=0; i<args.Length;i++)
				{
					file_name = args[i];
					//filename must not begin with '-'
					if(file_name[0] == '-')
						break;
					
					//open the file and append the data
					if( !File.Exists(file_name) ){
						Console.WriteLine("{0} does not exist, or a string was provided without the '-s' parameter", file_name);
					 	return;
					}
			        try
			        {
			            // Create an instance of StreamReader to read from a file.
			            // The using statement also closes the StreamReader.
			            using (StreamReader sr = new StreamReader(file_name))
			            {
			                string line, lines = "";
							lines =sr.ReadToEnd();
			                // Read lines from the file till the EOF
							//Incredibly slow!!
			                /*while ((line = sr.ReadLine()) != null)
			                {
								//todo: further processing options/filters for text based files
			                    lines += line;
			                }*/
							data += lines;
							//report result
							Console.WriteLine(	"Entropy of the file '{0}'({2} By) is: {1}",
												file_name , 
												EntropyShannon(lines, logbase, 0, arg_markov_m1).ToString("00.00"), 
												lines.Length
							);
							Console.WriteLine(	"The total length is: {1} By; Accumulative entropy is: {0}", 
												EntropyShannon(data, logbase, 0, arg_markov_m1).ToString("00.00"), 
												data.Length
							);
			            }
			        }
			        catch (Exception e)
			        {
			            Console.WriteLine("The file could not be read:");
			            Console.WriteLine(e.Message);
			        }
				}
			}
			//output information regarding the argument-string data
			if( index_arg_str >= 0 ){
				data = args[index_arg_str+1];
				Console.WriteLine(	"Entropy of the string({0}By) is: {1}",
									data.Length , 
									EntropyShannon(data, logbase).ToString("00.00")
				);
			}
			//output efficiency information
			if(arg_efficiency > 0){
				Console.WriteLine(	"Efficiency of the data({0}By) is: {1}",
									data.Length , 
									EntropyShannon(data, logbase, arg_efficiency).ToString("00.00")
				);
			}
			Console.WriteLine(	"Shannon Index of the data({0}By) is: {1} \nShannon's equitability (EH) is:{2} ",
								data.Length , 
								EntropyShannon(data, (float)Math.E, arg_efficiency).ToString("00.00"),
								EntropyShannon(data, (float)Math.E, 2).ToString("00.00")
			);
			/*
			 * PLOTTING
			 * */
			/* Print a histogram to the console; intentionallwon't plot anything if the string's length is below a length of 80 */
			int hist_width =  Entropy.getArg("-w") != null ? int.Parse(Entropy.getArg("-w")) : 60;
			int hist_height =  Entropy.getArg("-h")!= null ? int.Parse(Entropy.getArg("-h")) : 10;
			int seg_len = (int) (data.Length/hist_width);
			float max_ent = 0;
			var hist_seg = new Dictionary<int, float>();
			for(int i =0; i<hist_width; i++)
			{
				hist_seg[i] = (float) EntropyShannon( data.Substring(i*seg_len, seg_len), logbase, arg_efficiency, arg_markov_m1);
				if(hist_seg[i] > max_ent)
					max_ent = hist_seg[i];
			}
			Console.WriteLine("The maximum segmental-entropy ({0}By) is: {1}", seg_len, max_ent);
			Console.WriteLine();
			//normalize & plot; seg_ylen holds numeric y-axis scale
			float seg_ylen = (float) Math.Round((max_ent/hist_height), 2, MidpointRounding.AwayFromZero);
			//x/y plotting of the histogram
			for(int y =hist_height; y>=0; y--)
			{
				string con_str = (y*seg_ylen).ToString("0.00") + " | ";
				for(int x =0; x<hist_width; x++)
				{
					//normalize
					float coord_val = hist_seg[x]/(y*seg_ylen);
					//if the data value of the given row is above the current line height, plot it
					//if(hist_seg[x]
					con_str += (coord_val >= 1.0) ? "▓" : " "; //use # if problems with unicode char '▓' arise
				}
			   con_buffer += con_str +line_end;
				//Console.WriteLine(con_str);
				
			}
			//console footer i.e. x-axis
			string con_str_end = "";
			for(int x =0; x<hist_width; x++){ con_str_end += "-"; };
			con_buffer += con_str_end.PadLeft(hist_width+6, '-') +line_end;
			//Console.WriteLine( con_str_end.PadLeft(hist_width+6, '-') );
			con_str_end = "";
			for(int x =0, step =10; x<hist_width; x++)
			{
				if( x%step == 0) {
					//shorten the str a bit to acount for the space consumption of the scale-numbers
					if(con_str_end.Length > 10)
						con_str_end = con_str_end.Substring(0,con_str_end.Length-1);
					con_str_end +=(int)(100*x/hist_width) + "%";
				}else{
				 	con_str_end += " ";
				}
			};
			con_buffer += con_str_end.Trim().PadLeft(hist_width+2, ' ')+line_end;
			//Console.WriteLine( con_str_end.Trim().PadLeft(hist_width+2, ' ') );
			if( Array.IndexOf(args,"-o") != -1 ) 
			{
				string file_name_out;
				if( Array.IndexOf(args,"-o")+1 > args.Length)
					file_name_out = file_name+".out";
				else
					file_name_out = args[Array.IndexOf(args,"-o")+1]; //todo: allow -outfile: ||  Array.IndexOf(args,"-outfile")
				System.IO.StreamWriter fileStream = new System.IO.StreamWriter(file_name_out, true, System.Text.Encoding.UTF8); //only takes ASCI
				//byte[] bs = new byte[data.Length];
				//data.Read(bs, 0, (int)bs.Length);
				//fileStream.Write(bs, 0, (int)bs.Length);
				fileStream.WriteLine(con_buffer);
				fileStream.Close();
				
			}
			Console.WriteLine(con_buffer);
			
		}
		/// <summary>
		/// returns bits of entropy represented in a given string;
		/// @params: string s, float logbase = 0, int mode = 0(entropy)/1(efficiency)/2(equitability), int markovian = 0/1
		/// marovian implementation is imprecise
		/// </summary>
		public static float EntropyShannon(params object[] o) //C#4 allows default params: ...float logbase = 0.0f)
		{
			//set parameters
			string s = (string) o[0];
			float logbase = 256f; //2**8, alternatively ientr could be divided by 8
			int fnmode = 0, fnmarkov = 0;	//Default values
			if(o.Length >1)
				logbase = (float) o[1];
			if(o.Length >2)
				fnmode = (int) o[2];
			if(o.Length >3)
				fnmarkov = (int) o[3];
			
			//build character-histogram
			//in the case of fnmarkov, char-tuples are used
			var dict = new Dictionary<UInt32, int>();
			if(fnmarkov == 0)
			{
				//dict = new Dictionary<byte, int>();
				for(int i=0; i<s.Length; i++)
				{
					byte c = (byte)s[i];
			        if (!dict.ContainsKey(c))
			            dict.Add(c, 1);
			        else
			            dict[c] += 1;
				}
			}else if(fnmarkov == 1)
			{
				//dict = new Dictionary<UInt16, int>();
				for(int i=1; i<s.Length; i++) //todo: allow arbitrary pair lengths
				{
					byte cp = (byte)s[i-1]; //byte: UInt8
					byte ci = (byte)s[i];
					UInt16 tuple = (UInt16)(cp<<8); //(cp<<8)^ci  ...let's the parser complain
					tuple ^= ci;
			        if (!dict.ContainsKey(tuple))
			            dict.Add(tuple, 1);
					else
			            dict[tuple] += 1;
				}
			}else{
				//dict = new Dictionary<UInt16, int>();
				for(int i=2; i<s.Length; i++) //todo: allow arbitrary pair lengths
				{
					byte cp = (byte)s[i-2]; //byte: UInt8
					byte cm = (byte)s[i-1]; //byte: UInt8
					byte ci = (byte)s[i];
					UInt32 triple = (UInt32)(cp<<16);
					UInt16 tuple = (UInt16)(cm<<8);
					tuple ^= ci;
					triple ^= tuple;
			        if (!dict.ContainsKey(triple))
			            dict.Add(triple, 1);
					else
			            dict[triple] += 1;
				}
			}
			//Console.WriteLine("Number of unique species:{0}", dict.Count);
			//calculate the relative propability and weight by alphabet size -> entropy
		    float ientr = 0.0f;
		    int len = s.Length;
		    foreach (var val in dict.Values)
			{
		        var prop = (float)val / len;
				if(prop == 0)
					continue;
				
				if(fnmode == 0) 	//calculate the logbase-ary entropy
		        	ientr -= prop * (float) Math.Log(prop, logbase);
				else if(fnmode == 1) 				//calculate the logbase-ary efficiency
					ientr -= prop * (float) Math.Log(prop, logbase) / (float) Math.Log(len, logbase);
				else if(fnmode == 2)			//calculate Shannon's equitability
					ientr -= prop * (float) Math.Log(prop, logbase) / (float) Math.Log(dict.Count, Math.E);
		    }
		    return ientr;
		}
		/// <summary>
		/// alias function
		/// @params: string s, float logbase = 0
		/// </summary>

		public static float Efficiency(params object[] o)
		{
			return EntropyShannon(o, 1);
		
		}
		
		
	}
}
