#!/bin/bash

# this is a trap for ctrl + c 
trap ctrl_c INT

function ctrl_c() 
{
  echo "Trap: CTRL+C received, exit"
  exit
}