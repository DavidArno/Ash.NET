Ash.NET
=======

C#-based port of the AS3 entity framework, Ash. The original version of Ash is written by Richard Lord and can be found at https://github.com/richardlord/Ash.

Please note that this is an early push of the code: it absolutely is not yet fit for purpose. There is lots more work to be done in terms of:

1. Porting the remaining unit tests to C#
2. Tidying up the code & comments
3. Taking further advantage of C#'s features to improve the code
4. Porting Richard's Asteroid's game across to .NET.

Currently the code is in the form of a VS2012 solution. This should work in VS2010 too, but I haven't confirmed this. You will also need NuGet installed as it handles fetching the NUnit framework needed by the tests.