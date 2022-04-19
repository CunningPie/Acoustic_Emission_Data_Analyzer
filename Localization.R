library(grid)
library(gridExtra)
#install.packages("doSNOW")
#install.packages("foreach")
#library(foreach)
#library(doSNOW)

#cluster <- makeCluster(3, type = "SOCK", outfile="")
#registerDoSNOW(cluster)

#WIDTH <- 636  #c10
WIDTH <- 520   #k2

dataList <- read.csv("F:/Диплом/Курсовая/K2_waves.txt", sep = "", header = TRUE)

coords <- read.csv("F:/Диплом/Курсовая/K2_coords.txt", sep = " ", header = TRUE)
resFileName <- "F:/Диплом/Курсовая/K2_res.txt"

#dataList <- read.csv("F:/Диплом/Курсовая/K2_L600_2010_Waves.txt", sep = "", header = TRUE)
#coords <- read.csv("F:/Диплом/Курсовая/Координаты Датчиков 2010.txt", sep = " ", header = TRUE)

get_w <- function(num){
  return(dataList[which(dataList[, "Num"] == num), c("Time", "Channel")])
}

get_coords <- function(num){
  return(coords[num, c("x", "y")])
}

check_dist <- function(x, y, table)
{
  for (i in 1:(nrow(table) - 1))
    if (dist(x, y, table[i, "x"], table[i, "y"]) > dist(x, y, table[i + 1, "x"], table[i + 1, "y"]))
      return(FALSE)
  return(TRUE)
}

get_closest_sensor <- function(x, y)
{
  minDist <- integer.max
  resNum <- 1
    
  for (i in 1:nrow(coords))
  {
    d <- dist(x, y, coords[i, "x"], coords[i, "x"]) 
    if (d < minDist)
    {
      minDist <- d
      resNum <- i
    }
  }
}

sort_by_dist <- function(table, x, y)
{
  distVec <- c()
  
  for (i in 1:nrow(table))
  {
    distVec <- c(distVec, dist(x, y, table[i, "x"], table[i, "y"]))
  }
  
  return(order(distVec))
}

form_table <- function(num)
{
  wave <- get_w(num) 
  eq_table <- matrix(nrow = 0, ncol = 4)
  colnames(eq_table) <- c("ch", "t","x", "y")
  
  for (i in 1:nrow(wave))
  {
    ttt <- get_coords(wave[i, "Channel"])
    eq_table <- rbind(eq_table, c(wave[i, "Channel"], wave[i, "Time"], as.numeric(ttt$x), as.numeric(ttt$y)))
  }
  
  rownames(eq_table) <- c(1:nrow(eq_table))

  return(eq_table)
}

dist <- function(x0, y0, x1, y1)
{
  d1 <- sqrt((x1 - x0)^2 + (y1 - y0)^2)
  d2 <- sqrt((WIDTH - sqrt((x1-x0)^2))^2 + (y1 - y0)^2)
  
  if (d1 > d2)
    dres <- d2
  else
    dres <- d1
  
  return(dres)
}

eq <- function(x, y, x0, y0, x1, y1, t0, t1)
{
  return(abs((dist(x, y, x1, y1) - dist(x, y, x0, y0))) /(t1 - t0))
}

select_channels <- function(channels, table)
{
  d <- dist(table[1, "x"], table[1, "y"], table[2, "x"], table[2, "y"]) / 2
  
  xi_min <- min(table[, "x"]) - d 
  xi_max <- max(table[, "x"]) + d
  
  yi_min <- min(table[, "y"]) - d
  yi_max <- max(table[, "y"]) + d
  
  res <- matrix(ncol = 2, nrow = 0)
  
  for (i in 1:nrow(channels))
    if (channels[i, "x"] >= xi_min && channels[i, "x"] <= xi_max && channels[i, "y"] >= yi_min && channels[i, "y"] <= yi_max
        && any(channels[i, c("x", "y")] %in% table[, c("x", "y")] == FALSE) )
      res <- rbind(res, channels[i, ])
  
  return(res)
}

res_diam <- function(res)
{
  diag <- -1
  for (i in 1:nrow(res))
    for (j in 1:nrow(res))
  {
    d <- dist(res[i, "x"], res[i, "y"], res[j, "x"], res[j, "y"])
    
    if (d > diag)
      diag <- d
    }
  
  return(diag)
}

localize_event <- function(num, acc)
{
  res <- matrix(ncol = 3, nrow = 0)
  colnames(res) <- c("x", "y", "vel")
  table <- form_table(num)
  
  #xi_min <- min(table[, "x"])
  #xi_max <- max(table[, "x"])
  
  #yi_min <- min(table[, "y"])
  #yi_max <- max(table[, "y"])
  lthreshold <- 135
  uthreshold <- 410
  
  d <- dist(table[1, "x"], table[1, "y"], table[2, "x"], table[2, "y"])
  cat("approximate amount of iterations: ", d * d / 0.01, "\n")
  
  xi_min <- table[1, "x"] - d / 2
  xi_max <- table[1, "x"] + d / 2
  
  yi_min <- table[1, "y"] - d / 2
  yi_max <- table[1, "y"] + d / 2
  
  xi <- xi_min
  yi <- yi_min
  
  veli1 <- eq(xi, yi, table[1, "x"], table[1, "y"], table[2, "x"], table[2, "y"], table[1, "t"], table[2, "t"])
  veli2 <- eq(xi, yi, table[2, "x"], table[2, "y"], table[3, "x"], table[3, "y"], table[2, "t"], table[3, "t"])
  
  it <- 0
    
  while (xi <= xi_max)
  {
    while (abs(veli1 - veli2) > acc && yi <= yi_max)
    {
      yi <- yi + 0.1
      veli1 <- eq(xi, yi, table[1, "x"], table[1, "y"], table[2, "x"], table[2, "y"], table[1, "t"], table[2, "t"])
      veli2 <- eq(xi, yi, table[2, "x"], table[2, "y"], table[3, "x"], table[3, "y"], table[2, "t"], table[3, "t"])
      it <- it + 1
    }
    
    if (abs(veli1 - veli2) <= acc && veli1 >= lthreshold && veli1 <= uthreshold && check_dist(xi, yi, table))# 
    {
      cat("wave ", N, ":", xi, " ", yi, " ", veli1,"\n")
      res <- rbind(res, c(xi, yi, veli1))
      
      diam <- 2000
      if (nrow(res) > 1 && res_diam(res) > diam)
      {
        res <- res[0, ]
        cat("Diameter is bigger than ", diam, "\n")
        return(res)
      }
    }
    
    xi <- xi + 0.1
    yi <- yi_min
    
    veli1 <- eq(xi, yi, table[1, "x"], table[1, "y"], table[2, "x"], table[2, "y"], table[1, "t"], table[2, "t"])
    veli2 <- eq(xi, yi, table[2, "x"], table[2, "y"], table[3, "x"], table[3, "y"], table[2, "t"], table[3, "t"])
  }
  
  cat(it, "\n")
  return(res)
}

print_res <- function(res, resFileName, N)
{
  if (nrow(res) > 0)
  {
    table <- form_table(N)
    schannels <-select_channels(coords, table)
    
    cat("save file \n")
    rownames(res) <- 1:nrow(res)

    pdf(resFileName, width = 10)
    
    grid.newpage()
    grid.text("Coords: ", y = 0.8)
    grid.table(round(res, 2))
    #res <-res[, c("x", "y")]
    
    ttt <- dataList[which(dataList[, "Num"] == N), ][1, c("Num", "Event", "Channel", "Amplitude")]
    
    table_text <- paste("LE Num:", ttt["Num"], "Channel:", ttt["Channel"], "Amplitude:", ttt["Amplitude"]) 
    
    d <- dist(table[1, "x"], table[1, "y"], table[2, "x"], table[2, "y"]) / 2
    plot(table[, c("x", "y")], col = "blue",type="n", main=table_text, xlab = "x, cm", ylab = "y, cm", xlim = c(min(table[, "x"]) - d, max(table[, "x"]) + d), ylim = c(min(table[, "y"]) - d, max(table[, "y"] + d)), asp = 1)
    text(table[,c("x", "y")], label=paste(table[, "ch"], rownames(table), sep="/"), col = "green", pos=3)
    text(schannels[c("x", "y")], label=rownames(schannels), col='black', pos = 3)
    points(table[, c("x", "y")], col = "green", pch=16)
    points(schannels[c("x", "y")], col = "black", pch=16)
    
    points(res[, "x"], res[, "y"], col = "red", pch=15)
    
    dev.off()
  }
}

#print_res(filtered2Res[filtered2Res["N"] == 276, ], "wave_276.pdf", 276)
#max(dataList["Num"])

for (N in unique(dataList[, "Num"])) {
  cat("start: ", N,"\n")
  res <- localize_event(N, 0.01)
  print_res(res, resFileName, N)
  write.table(round(cbind(res, N), 2), resFileName, append = TRUE, quote = FALSE, sep = "\t", col.names = FALSE, row.names = FALSE)

  
#  if (nrow(res) > 0)
#  {
#    delete <- c()
#    for (i in 1:nrow(res))
#    {
#      sortedChannels <- sort_by_dist(coords, res[i, "x"], res[i, "y"])
      
#      #for (j in 1:nrow(table))
#      if (length(intersect(table[1:3, "ch"], sortedChannels[1:3])) == 0 )
#      {
#        cat(res[i, ], " deleted \n")
#        delete <- c(delete, i)
#      }
#    }
#    
#    if (!is.null((delete)))
#    {
#      res <- matrix(res[-delete, ], ncol=3)
#      colnames(res) <- c("x", "y", "vel")
#    }
#  }


  cat("finish: ", N, "\n")
}

filter1 <- function(results)
{
  Res <- matrix(nrow = 0, ncol = 4)
  
  for (i in 1:nrow(results))
  {
    sortedChannels <- sort_by_dist(coords, results[i, "x"], results[i, "y"])
    fixChannels <- form_table(results[i, "N"])
  
    #for (j in 1:nrow(table))
    if (length(intersect(fixChannels[1:3, "ch"], sortedChannels[1:3])) > 0 )
    {
      Res <- rbind(Res, results[i, ])
    }
  }
  
  return(Res)
}

filter2 <- function(results)
{
  Res <- matrix(nrow = 0, ncol = 4)
  
  for (i in 1:nrow(results))
  {
    sortedChannels <- sort_by_dist(coords, results[i, "x"], results[i, "y"])
    fixChannels <- form_table(results[i, "N"])
    
    #for (j in 1:nrow(table))
    if (length(intersect(fixChannels[1:3, "ch"], sortedChannels[1:3])) > 1 )
    {
      Res <- rbind(Res, results[i, ])
    }
  }
  
  return(Res)
}

filterHard <- function(results)
{
  Res <- matrix(nrow = 0, ncol = 4)
  
  for (i in 1:nrow(results))
  {
    sortedChannels <- sort_by_dist(coords, results[i, "x"], results[i, "y"])
    fixChannels <- form_table(results[i, "N"])
    
    #for (j in 1:nrow(table))
    if (fixChannels[1, "ch"] == sortedChannels[1] && fixChannels[2, "ch"] == sortedChannels[2])
    {
      Res <- rbind(Res, results[i, ])
    }
  }
  
  return(Res)
}

filterAmount <- function(results, amountBot, amountTop)
{
  Res <- matrix(nrow = 0, ncol = 4)
  
  for (i in unique(results[, "N"]))
  {
    n <- nrow(results[results["N"] == i, ])
    if ( n > amountBot && n <= amountTop)
    {
      Res <- rbind(Res, results[results["N"] == i, ])
    }
  }
  
  return(Res)
}

for (i in unique(filtered2Res[, "N"]))
{
  print_res(filtered2Res[filtered2Res["N"] == i, ], paste("K2_wave", i, ".pdf"), i)
}


allPoints <- read.csv(resFileName, sep = "\t", header = TRUE)

filtered1Res <- filter1(allPoints)
filtered2Res <- filter2(allPoints)
filteredHardRes <- filterHard(allPoints)
filteredAmount <- filterAmount(filtered2Res, 0, 4)

filter_result(filtered2Res, coords, "all_points.pdf")


filter_result <- function(data, coords, fileName)
{
  N1 <- filterAmount(data, 0, 1)
  N3 <- filterAmount(data, 1, 3)
  N3m <- filterAmount(data, 3, 4)
  pdf(fileName, width = 10)
  
  plot(coords, col = "black", type="n", xlab = "x, cm", ylab = "y, cm", asp = 1, ylim=c(-50, 370), xlim=c(-330, 330))
  title("Green: n = 1; blue: 1 < n <= 3; red: n = 4 (n - amount of potential positions for one acoustic wave)", adj = 1, cex.main = 0.8, font.main = 1, col.main = "black")
  points(N3m[, "x"], N3m[, "y"], col = rgb(1, 0, 0), pch=16)
  points(N3[, "x"], N3[, "y"], col = rgb(0, 0, 1), pch=16)
  points(N1[, "x"], N1[, "y"], col = rgb(0, 1, 0), pch=16)
  text(coords, label=rownames(coords), col='black')
  
  dev.off()
}
