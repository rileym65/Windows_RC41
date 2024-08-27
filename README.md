HP-41CV simulator.
This project is based upon my Linux command line Rc41, except for having an actual calculator UI as opposed to the
command line interface.  

This program simulates an HP-41CV calculator.  This program does not emulate the nut processor, nor does it use any
of the original HP-41 software.  This is a clean-room implementation of the HP-41.  It interprets the original calculator's
byte codes and memory/register usage, and simulates as closely as possible the user interface.

In addition to simulating the HP-41CV calculator, the 82143 printer, 82104 card reader, and 82161 tape drive (using commands
from the 82160 IL module) are also simulated.

Goal is for a simulation that exactly replicates the original calculator, including the use of synthetic programming.

At this point the simulation is mostly operational and most functions work, there are still some idiosyncocies in how
some functions work and there other features (like key preview) that do not function yet.

Note about synthetic programming.
Since this simulation does not use the original HP41 software, the techniques normally used to create the byte-jumper,
upon which synthetic programming relies on, cannot be used on this simulator.  There are two options for setting up
for synthetic programming.  Option 1. there is a synth.crd card file that contains a number of synthetic key assignements,
including the byte jumper.  Option 2, on the debug panel, there is a button called "Synth" that allows you to assign
synthetic functions to keys.
