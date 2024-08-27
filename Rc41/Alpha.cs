using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41
{
    partial class Cpu
    {
        /*
        public void ProgramAlpha(int key)
        {
            if (FlagSet(F_SHIFT))
            {
                ram[REG_R + 0] = keys[key].salpha;
                if (ram[REG_R + 0] == 0x81) ram[REG_R + 0] = 0x7f;
                ClearFlag(F_SHIFT);
                window.Shift(false);
            }
            else
                ram[REG_R + 0] = keys[key].alpha;
            if (FlagSet(F_ALPHA_IN))
            {
                if (ram[REG_P + 6] >= 15) return;
                ram[REG_P + 6]++;
                ram[REG_R + 1] = (byte)(0xf0 + ram[REG_P + 6]);
            }
            else
            {
                ram[REG_P + 6] = 1;
                ram[REG_R + 1] = 0xf1;
            }
            ProgramStep("");
            window.Display(Display());
        }

        public void AlphaDown(int key)
        {
            if (FlagSet(F_PRGM))
            {
                ProgramAlpha(key);
                return;
            }

            if (FlagSet(F_SHIFT))
            {
                if (keys[key].salpha < 0x80)
                {
                    if (ram[REG_P + 2] != 0) return;
                    if (!FlagSet(23))
                    {
                        for (var i = REG_M; i <= REG_P + 6; i++)
                            ram[i] = 0x00;
                        SetFlag(23);
                    }
                    for (var i = REG_P + 6; i > REG_M; i--)
                        ram[i] = ram[i - 1];
                    ram[REG_M] = keys[key].salpha;
                    window.Display(Display());
                    ClearFlag(47);
                    window.Shift(false);
                }
                else
                {
                    if (keys[key].salpha == 0x81)
                    {
                        SetFlag(23);
                        window.Display(Display());
                        ClearFlag(47);
                        window.Shift(false);
                    }
                    if (keys[key].salpha == 0x86)
                    {
                        pendingKey = key;
                    }
                    if (keys[key].salpha == 0x88)
                    {
                        pendingKey = key;
                    }
                    if (keys[key].salpha == 0x82)
                    {
                        pendingKey = key;
                    }
                    if (keys[key].salpha == 0x83)
                    {
                        pendingKey = key;
                    }

                }
            }
            else
            {
                if (keys[key].alpha < 0x80)
                {
                    if (ram[REG_P + 2] != 0) return;
                    if (!FlagSet(23))
                    {
                        for (var i = REG_M; i <= REG_P + 6; i++)
                            ram[i] = 0x00;
                        SetFlag(23);
                    }
                    for (var i = REG_P + 6; i > REG_M; i--)
                        ram[i] = ram[i - 1];
                    ram[REG_M] = keys[key].alpha;
                    window.Display(Display());
                }
                else
                {
                    if (keys[key].alpha == 0x85)
                    {
                        pendingKey = key;
                    }
                    if (keys[key].alpha == 0x87)
                    {
                        pendingKey = key;
                    }

                }
            }
        }

        public void AlphaUp(int key)
        {

            if (!FlagSet(F_SHIFT) && keys[key].alpha == 0x87)             // R/S
            {
                if (uiMode == 0)
                {
                    if (!running)
                    {
                        if (FlagSet(22)) EndNumber();
                        running = true;
                        window.RunTimerEnabled(true);
                        goose = "\x81           ";
                        window.Display(goose);
                        return;
                    }
                }
                return;
            }


            if (FlagSet(F_SHIFT))
            {
                if (keys[key].salpha == 0x86)
                {
                    for (var i = REG_M; i <= REG_P + 2; i++)
                        ram[i] = 0x00;
                    ClearFlag(23);
                    window.Display(Display());
                }
                if (keys[key].salpha == 0x82)
                {
                    SetupMode1(27);
                    ClearFlag(F_ALPHA);
                    ClearFlag(F_ALPHA_IN);
                    window.Alpha(false);
                }
                if (keys[key].salpha == 0x83)
                {
                    SetupMode1(23);
                    ClearFlag(F_ALPHA);
                    ClearFlag(F_ALPHA_IN);
                    window.Alpha(false);
                }
                if (keys[key].salpha == 0x88)
                {
                    ram[REG_R + 1] = 0x7e;
                    ram[REG_R + 0] = 0x00;
                    ClearFlag(F_ALPHA);
                    ClearFlag(F_ALPHA_IN);
                    window.Alpha(false);
                    Execute();
                }
                ClearFlag(F_SHIFT);
                window.Shift(false);
            }
            else
            {
                if (keys[key].alpha == 0x85)
                {
                    if (FlagSet(23))
                    {
                        ClearFlag(23);
                        for (var i = REG_M; i <= REG_P + 2; i++)
                            if (ram[i] != 0x00) SetFlag(23);
                        for (var i = REG_M; i < REG_P + 2; i++)
                            ram[i] = ram[i + 1];
                        ram[REG_P + 2] = 0x00;
                    }
                    else
                    {
                        for (var i = REG_M; i <= REG_P + 2; i++)
                            ram[i] = 0x00;
                        ClearFlag(23);
                    }
                    window.Display(Display());
                }
            }
        }
        */
    }
}
