<img src="https://lh5.googleusercontent.com/-Jd8x1W5KoNo/T_AhYh2yBgI/AAAAAAAAA7k/lTf7p_Vz-7s/s800/ent_logo.png" style="border:0px; margin:10px; margin-right:30px; float:left;">


#ENT - view string metrics and entropy of arbitrary files
##### *As a command line tool, .NET library or web-application (coming)*

---

**author**: lo sauer 2011-12; www.lsauer.com   
**website**: https://github.com/lsauer/entropy   
**license**: MIT license   
**description**: quickly plot entropy information and string metrics of arbitrary files or 
strings from the console Input/Output   

### Analysing a file:

<img src="https://googledrive.com/host/0ByqWUM5YoR35SUREUWdZcTRiQ3M/ent_v0812_anim.Gif">


### Cross Platform:

<img src="https://googledrive.com/host/0ByqWUM5YoR35SUREUWdZcTRiQ3M/ent_running_ubuntu80.png">


<example>

 * 
	**Windows Usage**: 	


```bash
    > type file1.ext file2.ext file3.ext | ent -b 2.15
    
    > "teststringdata" | ent -b 2.15 -s
```

 *	
	**Linux, Mac Usage**: 

```bash
    $ cat file1.ext file2.ext file3.ext | ent -b 2.15
    
    $ "sometextdata" | ent -b 64 -s
```


</example>


###Background


In information theory, *entropy* is a measure of the uncertainty associated with a random variable. Informatically, the term usually refers to the **Shannon entropy**, which quantifies the expected value of the information contained in a message (*a specific instance of the random variable*), in units of bits. (See: [Entropy](http://en.wikipedia.org/wiki/Entropy_(information_theory)) )

A file with high entropy shows few repeated patterns and is typically compressed/optimized.  Such a highly compressed data-stream will feature an entropy index of greater *>5* for a binary-base 2.

###Purpose

The purpose of this tool is quickly analyzing arbitrary files, for instance *biological sequence files* or *serialized JSON data*. Naturally powerful investigation options are granted to researchers in the form of the *[R-statistical language](http://www.r-project.org/ "The R Project for Statistical Computing")* or *[Matlab](http://www.mathworks.com/products/matlab/)* and *[Wolfram Mathematica](http://www.wolfram.com/mathematica/)*.

**Ad hoc investigation** however is much faster with a dedicated command line tool along and the **features the console environment** provides such as **autocompletion** (`pressing Tab`). Virtually **no startup times** are required and plots can be output in web-compatible **vector graphics**.

### Usage
```bash
    Usage: shantropy [<filename1> <fname2>...1st param!] [-f fromBy] [-t toBy] [-o <outfile>] [-h help]
    [-e efficiency] [-m 1,2.. 1st,2nd order markov] [-b base-alphabet]
    [-w width plot] [-h height plot] [-z zoom%] [-fp fileposition]
    [-p plot permutation entropy] [-s <string> as last param!]
    Press CTRL+C or Q to Quit!
    Press PAGEUP / PAGEDOWN to zoom in or out of the file
    Press LEFT / RIGHT Arrow to navigate to the next or previous file-segment
```

### Parameters
- **-m 0** zero-order Markov source: *default* (pratically identical with Shannon entropy when the log-base is 2)
- **-m 1** first-order Markov source: ...number of linked characters is one
- **-m 2** second-order Markov source e.g. `ent -m 2`
- **-m `<n>`** n order markov source
- **-b `<decimal>`** "b-ary entropy": a different base can be set with e.g. `ent -b 2,15` , default is 256 for ASCII; use 64 for literature-text
- **-s `<stringdata>`** arbitrary string passing: *-s* must be passed as the last argument!
- **-w `<int>`** width of the plot
- **-h `<int>`** height of the plot
- **-f,-t `<int>`** define a file segment in Byte (from/to). Both are optional
- **-z** file-segment zoom in percent
- **-fp** file-segment position (0-n)
- **-o outfile** plot data to a given file and create or append to the file `outfile`,
 - use `ent .... > myfilen.out` to capture the entire console output
 - use `ent .... > mygraphics.svg` to plot to an svg file
- **-e** ...plot the efficiency of the data
- **-p** ...plot and compute the permutation efficiency

*note:* files have to be passed as first arguments:
To calculate metrics for several files put them in sequence e.g. `ent explain.nfo markdownsharp-20100703-v113.7z -b 3,6`


###Todo: 
- make and use an console argument hash map or struct params
- code cleanup

###Fixes
- slow -> fixed; `Readline loop` was replaced by ReadAll; up to 200x speedup
- navigation of the file for chunked data processing
- incorrect results -> fixed: for text files set -b 64, to get meaninful results

###Example
Example for a typical info (*.nfo*) file:
the ordinate(y-axis) shows the entropy and the abscisse (x-axis) shows the file-segment position in percent

*result:* the text is highly compressible and clearly shows structuring

```
0,60 |                                  ▓▓▓▓▓▓▓▓▓      ▓▓▓▓▓
0,54 |                                  ▓▓▓▓▓▓▓▓▓    ▓▓▓▓▓▓▓    ▓▓
0,48 |                                  ▓▓▓▓▓▓▓▓▓    ▓▓▓▓▓▓▓    ▓▓▓
0,42 |           ▓    ▓▓  ▓             ▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓    ▓▓▓
0,36 |           ▓    ▓▓▓▓▓    ▓        ▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓    ▓▓▓
0,30 |           ▓▓   ▓▓▓▓▓    ▓ ▓      ▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓    ▓▓▓
0,24 | ▓       ▓ ▓▓ ▓ ▓▓▓▓▓  ▓ ▓ ▓   ▓ ▓▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓ ▓  ▓▓▓
0,18 | ▓       ▓▓▓▓ ▓ ▓▓▓▓▓  ▓▓▓ ▓▓  ▓ ▓▓▓▓▓▓▓▓▓▓▓ ▓ ▓▓▓▓▓▓▓ ▓  ▓▓▓
0,12 | ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ▓▓▓▓▓ ▓▓▓▓▓▓▓▓▓▓▓ ▓ ▓▓▓▓▓▓▓ ▓  ▓▓▓
0,06 | ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ▓▓▓▓▓ ▓▓▓▓▓▓▓▓▓▓▓▓▓ ▓▓▓▓▓▓▓ ▓▓ ▓▓▓
0,00 | ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ▓▓▓▓▓▓▓▓▓▓▓▓▓ ▓▓▓▓▓▓▓ ▓▓▓▓▓▓
------------------------------------------------------------------
     0%        16%        33%        50%        66%        83%
```

###Useful links: 
- http://www.lsauer.com/2011/09/ent-plotting-entropy-and-computing.html
- http://www.tiem.utk.edu/~mbeals/shannonDI.html
- http://books.google.com/books?id=IxrjpbNH2XAC&pg=PA17&lpg=PA17&dq=second+order+markov+source

###Case studies:
- http://www.lsauer.com/2011/10/google-chrome-session-restore-web.html
- http://www.lsauer.com/2012/06/permutation-entropy-plot-added-to-ent.html
- http://www.lsauer.com/2012/06/linkedin-lastfm-eharmony-analysis-of.html


Fork it on github: https://github.com/lsauer/entropy

Have fun! In fact don't use this program for anything else yet...
