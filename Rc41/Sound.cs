﻿using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41
{
    public class Sound
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Beep(uint dwFreq, uint dwDuration);

        uint[,] tones = new uint[128,2]
        {
            { 175, 280 },               // 00  0      
            { 197, 280 },               // 01  1
            { 225, 280 },               // 02  2
            { 263, 280 },               // 03  3
            { 315, 280 },               // 04  4
            { 394, 280 },               // 05  5
            { 525, 280 },               // 06  6
            { 629, 280 },               // 07  7
            { 788, 280 },               // 08  8
            { 1051, 280 },              // 09  9
            { 105, 2200 },              // 0a  10      
            { 113, 2200 },              // 0b  11
            { 121, 2700 },              // 0c  12
            { 131, 3500 },              // 0d  13
            { 143, 800 },               // 0e  14
            { 158, 2300 },              // 0f  15

            { 175, 2000 },              // 10  0      
            { 197, 340 },               // 11  1
            { 225, 1500 },              // 12  2
            { 263, 330 },               // 13  3
            { 315, 500 },               // 14  4
            { 394, 1000 },              // 15  5
            { 525, 450 },               // 16  6
            { 629, 840 },               // 17  7
            { 788, 300 },               // 18  8
            { 1051, 550 },              // 19  9
            { 105, 5000 },              // 1a  10      
            { 113, 3500 },              // 1b  11
            { 121, 2000 },              // 1c  12
            { 131, 4100 },              // 1d  13
            { 143, 300 },               // 1e  14
            { 158, 2400 },              // 1f  15

            { 175, 25   },              // 20  0      
            { 197, 1130 },              // 21  1
            { 225, 2350 },              // 22  2
            { 263, 2000 },              // 23  3
            { 315, 1350 },              // 24  4
            { 394, 23   },              // 25  5
            { 525, 23   },              // 26  6
            { 629, 350  },              // 27  7
            { 788, 700  },              // 28  8
            { 1051, 520 },              // 29  9
            { 105, 850  },              // 2a  10      
            { 113, 450  },              // 2b  11
            { 121, 3200 },              // 2c  12
            { 131, 180  },              // 2d  13
            { 143, 1360 },              // 2e  14
            { 158, 130  },              // 2f  15

            { 175, 540  },              // 30  0      
            { 197, 370  },              // 31  1
            { 225, 2100 },              // 32  2
            { 263, 1950 },              // 33  3
            { 315, 280  },              // 34  4
            { 394, 150  },              // 35  5
            { 525, 800  },              // 36  6
            { 629, 770  },              // 37  7
            { 788, 650  },              // 38  8
            { 1051, 58 },               // 39  9
            { 105, 420  },              // 3a  10      
            { 113, 410  },              // 3b  11
            { 121, 3300 },              // 3c  12
            { 131, 390  },              // 3d  13
            { 143, 970  },              // 3e  14
            { 158, 300  },              // 3f  15

            { 175, 1880 },              // 40  0      
            { 197, 2350 },              // 41  1
            { 225, 400  },              // 42  2
            { 263, 240  },              // 43  3
            { 315, 1050 },              // 44  4
            { 394, 290  },              // 45  5
            { 525, 32   },              // 46  6
            { 629, 240  },              // 47  7
            { 788, 140  },              // 48  8
            { 1051, 150 },              // 49  9
            { 105, 3700  },             // 4a  10      
            { 113, 300  },              // 4b  11
            { 121, 3760 },              // 4c  12
            { 131, 3400 },              // 4d  13
            { 143, 890  },              // 4e  14
            { 158, 900  },              // 4f  15

            { 175, 85   },              // 50  0      
            { 197, 220  },              // 51  1
            { 225, 1750 },              // 52  2
            { 263, 740  },              // 53  3
            { 315, 280  },              // 54  4
            { 394, 1250 },              // 55  5
            { 525, 150  },              // 56  6
            { 629, 140  },              // 57  7
            { 788, 580  },              // 58  8
            { 1051, 50  },              // 59  9
            { 105, 2700 },              // 5a  10      
            { 113, 420  },              // 5b  11
            { 121, 3210 },              // 5c  12
            { 131, 2950 },              // 5d  13
            { 143, 300  },              // 5e  14
            { 158, 2400 },              // 5f  15

            { 175, 650  },              // 60  0      
            { 197, 2320 },              // 61  1
            { 225, 430  },              // 62  2
            { 263, 1250 },              // 63  3
            { 315, 120  },              // 64  4
            { 394, 1000 },              // 65  5
            { 525, 990  },              // 66  6
            { 629, 840  },              // 67  7
            { 788, 700  },              // 68  8
            { 1051, 520 },              // 69  9
            { 105, 230  },              // 6a  10      
            { 113, 450  },              // 6b  11
            { 121, 3620 },              // 6c  12
            { 131, 330  },              // 6d  13
            { 143, 2100 },              // 6e  14
            { 158, 350  },              // 6f  15

            { 175, 1700 },              // 70  0      
            { 197, 650  },              // 71  1
            { 225, 1450 },              // 72  2
            { 263, 520  },              // 73  3
            { 315, 1250 },              // 74  4
            { 394, 1300 },              // 75  5
            { 525, 240  },              // 76  6
            { 629, 840  },              // 77  7
            { 788, 140  },              // 78  8
            { 1051, 330 },              // 79  9
            { 105, 250  },              // 7a  10      
            { 113, 4600 },              // 7b  11
            { 121, 760  },              // 7c  12
            { 131, 4000 },              // 7d  13
            { 143, 3500 },              // 7e  14
            { 158, 2900 },              // 7f  15
        };
        public void PlayBeep()
        {
            Beep(525, 280);
        }

        public void PlayTone(int n)
        {
            n = n & 0x7f;
            Beep(tones[n, 0], tones[n, 1]);
        }
    }
}
