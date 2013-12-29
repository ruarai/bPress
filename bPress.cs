using System;
using System.Collections.Generic;

namespace bPress
{
    /// <summary>
    /// Provides an interface for using the bPress compression algorithm.
    /// </summary>
    static class bPress
    {
        /// <summary>
        /// Compresses the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes to be compressed.</param>
        /// <returns>The compressed bytes.</returns>
        public static IEnumerable<byte> Compress(IEnumerable<byte> bytes)
        {
            IList<byte> outBytes = new List<byte>();//The output buffer

            var prevByte = new byte();//A 'last' byte to check if the byte currently being checked is equivalent to the previous one
            bool firstRun = true;

            int repeats = 0;//How many times the 'last' byte has been the same as the current one
            foreach (byte b in bytes)
            {
                if (!firstRun)//Hacky way to stop the first run from having a null from the default value of prevByte
                {
                    if (b == prevByte)
                    {
                        repeats++;
                    }
                    else
                    {
                        while (repeats > Byte.MaxValue) //We cant have bytes over the value of 255. 
                        {
                            outBytes.Add(prevByte);
                            outBytes.Add(Byte.MaxValue);
                            repeats = repeats - Byte.MaxValue;
                        }
                        outBytes.Add(prevByte);
                        outBytes.Add((byte)repeats);

                        repeats = 0; //Reset the repeats
                    }
                }
                prevByte = b;
                firstRun = false;
            }

            //We'll need to finalise the last byte in the input array too.
            while (repeats > Byte.MaxValue)
            {
                outBytes.Add(prevByte);
                outBytes.Add(Byte.MaxValue);
                repeats = repeats - Byte.MaxValue;
            }

            outBytes.Add(prevByte);
            outBytes.Add((byte)repeats);

            return outBytes;
        }

        /// <summary>
        /// Decompresses the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes to be compressed.</param>
        /// <returns>Returns the decompressed bytes.</returns>
        public static IEnumerable<byte> Decompress(IList<byte> bytes)
        {
            if (bytes.Count % 2 != 0)//If bytes arent of an even count
            {
                Console.WriteLine("Failed to decompress as file does not have an even byte count.");
                return new List<byte>();
            }

            IList<byte> outBytes = new List<byte>();

            for (int i = 0; i < bytes.Count; i++)
            {
                byte target = bytes[i];
                i++;
                byte repeats = bytes[i];

                for (int r = 0; r < repeats + 1; r++)
                {
                    outBytes.Add(target);
                }
            }

            return outBytes;
        }
    }
}
