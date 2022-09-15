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
    BlueLobby = 512,
    RedLobby = 1024,
    BlueShipInside = 2048,
    RedShipInside = 4096,
    BlueShip = 8192,
    RedShip = 16384,

    OutSide = Forest | Field | Altar | BlueLobby | RedLobby | BlueShip | RedShip | Beach,
    InSide = Cave | EngineRoom | ChargeRoom | BatteryRoom | BottleRoom | BlueShipInside | RedShipInside,

    Ship = RedShip | RedShipInside | BlueShip | BlueShipInside,
    SoilGround = Field | Forest,
    ConcreteGround = Cave | EngineRoom | ChargeRoom | BatteryRoom | BottleRoom | BlueLobby | RedLobby
}
