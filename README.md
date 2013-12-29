bPress
======

A simple compression algorithm written in C# that works best with data with little patterns but that still has large amounts of repeating of bytes. This gives it little use, but this was mainly just an experiment. This has probably also been done before, but I haven't seen any that use this technique.

Basically, the following pseudo-data:

HHHHHHHHHBBBBBBBBBBBDDDDDDDDDAAABBBBBBBBBBBCC

Would be compressed into this:

H8 B10 D8 A2 B10 C1

Where each pair is a representation of how many times that particular "byte" repeats. This is limited to 255, and when that limit is passed it will create multiple capped at 255 until the repetition ends. For example, a byte array that repeats 250 times would shown like this:

B255 B5

Unfortunately, with data that does not repeat, data sizes can easily be double in size.

