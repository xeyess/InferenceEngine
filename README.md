# InferenceEngine
 A C# command-line program developed to interpret logical expressions. 
 *Used in an assignment.
 
 ## Main
 The application is located in the debug folder in the bin.
In this folder there will be:
- InferenceEngine.exe (the application to be used in the command line)
- test_HornKB.txt (the sample definition provided)
- There are also .bat files that will automatically test 
		  the program using the labelled method name and the sample file
- cmd.exe (the command prompt executable for ease of access)

The format for the command-line should be
	- InferenceEngine.exe *method* *filename.txt*

The algorithms implemented are...
Name | Method
------------ | -------------
Truth Table Checking | TT
Forward Chaining | FC
Backwards Chaining | BC

For example, InferenceEngine.exe  TT test_HornKB.txt
would run the Truth Table Checking algorithm on the test_HornKB.txt file.

Important Resources were:
- AI - A Modern Approach 3rd edition
- http://www.iiia.csic.es/~puyol/IAGA/Teoria/07-AgentsLogicsII.pdf
- http://vlm1.uta.edu/~athitsos/courses/cse4308_fall2015/lectures/03a_tt_entails.pdf
