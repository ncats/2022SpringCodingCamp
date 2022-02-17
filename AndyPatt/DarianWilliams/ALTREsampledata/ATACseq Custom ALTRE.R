setwd("~/2022SpringCodingCamp/DarianWilliams/ALTREsampledata")
library(ALTRE)
library(GenomicRanges)
csvfile <- loadCSVFile("DNaseEncodeExample.csv")
csvfile
loadBedFiles_custom <- function (csvfile) 
{
  if (!is(csvfile, "data.frame")) 
    stop("csvfile must be a data.frame ")
  readBed <- function(bedPath, ind) {
    trackline <- utils::read.table(bedPath, nrows = 1, sep = "\t")
    if (length(grep("track type", trackline$V1)) == 
        0) {
      bed <- DataFrame(readr::read_delim(bedPath, delim = "\t", 
                                         col_names = FALSE, na = "."))[, 1:3]
    }
    else {
      bed <- DataFrame(readr::read_delim(bedPath, delim = "\t", 
                                         col_names = FALSE, na = ".", skip = 1))[, 
                                                                                 1:3]
    }
    colnames(bed) <- c("seqnames", "start", "end")
    bed <- DataFrame(bed, csvfile[ind, c("sample", 
                                         "replicate")])
    bed <- within(bed, {
      start <- start + 1L
      sample <- factor(sample)
      replicate <- factor(replicate)
    })
    return(bed)
  }
  bedFilesPath <- file.path(csvfile$datapath, csvfile$peakfiles)
  bedFiles <- mapply(readBed, bedFilesPath, seq_along(bedFilesPath))
  names(bedFiles) <- paste(csvfile$sample, csvfile$replicate, 
                           sep = "_")
  hotspots <- lapply(bedFiles, function(x) as(x, "GRanges"))
  return(GRangesList(hotspots, compress = FALSE))
}

samplePeaks <- loadBedFiles(csvfile)
samplePeaks
consensusPeaks <- getConsensusPeaks(samplepeaks = samplePeaks,
                                    minreps = 2)