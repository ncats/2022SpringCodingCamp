##Need to make a file of Transcription Start Sites (TSS) for ALTRE *getTSS* function?##

By default ALTRE works on the hg19 human genome. However, the __new update (01/25/2017)__ enables ALTRE to easily take on other human builds or other organisms entirely. However, it is up to users to provide their own file of gene transcription start sites to supply to the *getTSS()* function. This file can be created from a gtf file.

The *getTSS()* function requires the transcription start sites to be in the following format: chr, start, stop, gene_name. A gtf is not in this format, and may contain additional information besides TSS, such as the start and stop sites of ALL exons in EVERY gene in the genome. These need to be filtered out. 

The perl file available in this folder converts a human hg19 gtf file to the bed file you can use in the ALTRE R package (in the *getTSS* function). 

The example gtf file is too large to host on a github free repository. 
The gtf file can be download here: ftp://ftp.ensembl.org/pub/release-75/gtf/homo_sapiens/Homo_sapiens.GRCh37.75.gtf.gz

Basically, I decided that TSS for me would be the start coordinates of the 1st exon of a gene. In the perl script I parse through the all the data looking for all the exon 1s and placing them in the bed file. 

How to run the script:

```{R}
perl ./gtftobed_human.pl [inputgtf] [outputfile]
```

```{R}
perl ./gtftobed_human.pl Homo_sapiens.GRCh37.75.gtf Homo_sapiens.GRCh37.75_exon1only.bed
```

The script is customized to this gtf file. It may not work on a gtf file of another species or even a different human build. 

However, with minor modifications (on your part!) it can tackle other gtf files and so it is a good starting place. 

When you have the correct bed file it can be supplied to the *getTSS()* function: 


```{R}
TSSannot <- getTSS(file="Homo_sapiens.GRCh37.75_exon1only.bed")
```
