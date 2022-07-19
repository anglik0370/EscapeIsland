using System;

[Flags]
public enum Area
{
    None = 0,
    Cave = 1,
    Forest = 2,
    Beach = 4,
    Field = 8,
    Altar = 16,
    EngineRoom = 32,
    ChargeRoom = 64,
    BatteryRoom = 128,
    BottleRoom = 256,
    Lobby = 512,
    ShipInside = 1024,
    Ship = 2048,

    OutSide = Forest | Field | Altar | Lobby | Ship,
    InSide = Cave | EngineRoom | ChargeRoom | BatteryRoom | BottleRoom | ShipInside,
}
