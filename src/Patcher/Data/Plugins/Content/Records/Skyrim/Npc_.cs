/// Copyright(C) 2015 Unforbidable Works
///
/// This program is free software; you can redistribute it and/or
/// modify it under the terms of the GNU General Public License
/// as published by the Free Software Foundation; either version 2
/// of the License, or(at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program; if not, write to the Free Software
/// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using Patcher.Data.Plugins.Content.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Plugins.Content.Records.Skyrim
{
    [Record(Names.NPC_)]
    [Game(Games.Skyrim)]
    public sealed class Npc_ : GenericFormRecord, IFeaturingObjectBounds, IFeaturingScripts
    {
        [Member(Names.VMAD)]
        public VirtualMachineAdapter VirtualMachineAdapter { get; set; }
        
        [Member(Names.OBND)]
        [Initialize]
        public ObjectBounds ObjectBounds { get; set; }

        [Member(Names.ACBS)]
        public NpcConfiguration Configuration {get; set;}
        
        [Member(Names.SNAM)]
        [Initialize]
        public List<FactionField> Factions {get; set;}

        [Member(Names.INAM)]
        [Reference(Names.LVLI)]
        public uint DeathItem { get; set; }

        [Member(Names.VTCK)]
        [Reference(Names.VTYP)]
        public uint Voice { get; set; }

        [Member(Names.TPLT)]
        [Reference(Names.LVLN + Names.NPC_)]
        public uint Template { get; set; }

        [Member(Names.RNAM)]
        [Reference(Names.RACE)]
        public uint Race { get; set; }

        //SPCT
        //SPLO
        //DEST

        [Member(Names.WNAM)]
        [Reference(Names.ARMO)]
        public uint WornArmor { get; set; }

        [Member(Names.ANAM)]
        [Reference(Names.ARMO)]
        public uint FarAwayModel { get; set; }

        [Member(Names.ATKR)]
        [Reference(Names.RACE)]
        public uint AttackRace { get; set; }

        //Attacks

        [Member(Names.SPOR)]
        [Reference(Names.FLST)]
        public uint SpectatorOverridePackageList { get; set; }

        [Member(Names.OCOR)]
        [Reference(Names.FLST)]
        public uint ObserveDeadBodyOverridePackageList { get; set; }

        [Member(Names.GWOR)]
        [Reference(Names.FLST)]
        public uint GuardWarnOverridePackageList { get; set; }

        [Member(Names.ECOR)]
        [Reference(Names.FLST)]
        public uint CombatOverridePackageList { get; set; }
        
        [Member(Names.PRKZ)]
        public uint PerkCount { get; set; }
        
        [Member(Names.PRKR)]
        [Initialize]
        public List<PerkField> Perks {get; set;}

        public sealed class NpcConfiguration: Field
        {
            public uint Flags;
            public short MagickaOffset;
            public short StaminaOffset;
            public short Level;
            public ushort CalcMinLevel;
            public ushort CalcMaxLevel;
            public ushort SpeedMultiplier;
            public short DispositionBase;
            public ushort TemplateFlags;
            public short HealthOffset;
            public ushort BleedoutOverride;

            internal override void ReadField(RecordReader reader)
            {
                Flags = reader.ReadUInt32();
                MagickaOffset = reader.ReadInt16();
                StaminaOffset = reader.ReadInt16();
                Level = reader.ReadInt16();
                CalcMinLevel = reader.ReadUInt16();
                CalcMaxLevel = reader.ReadUInt16();
                SpeedMultiplier = reader.ReadUInt16();
                DispositionBase = reader.ReadInt16();
                TemplateFlags = reader.ReadUInt16();
                HealthOffset = reader.ReadInt16();
                BleedoutOverride = reader.ReadUInt16();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(Flags);
                writer.Write(MagickaOffset);
                writer.Write(StaminaOffset);
                writer.Write(Level);
                writer.Write(CalcMinLevel);
                writer.Write(CalcMaxLevel);
                writer.Write(SpeedMultiplier);
                writer.Write(DispositionBase);
                writer.Write(TemplateFlags);
                writer.Write(HealthOffset);
                writer.Write(BleedoutOverride);
            }

            public override Field CopyField()
            {
                return new NpcConfiguration()
                {
                    Flags = this.Flags,
                    MagickaOffset = this.MagickaOffset,
                    StaminaOffset = this.StaminaOffset,
                    Level = this.Level,
                    CalcMinLevel = this.CalcMinLevel,
                    CalcMaxLevel = this.CalcMaxLevel,
                    SpeedMultiplier = this.SpeedMultiplier,
                    DispositionBase = this.DispositionBase,
                    TemplateFlags = this.TemplateFlags,
                    HealthOffset = this.HealthOffset,
                    BleedoutOverride = this.BleedoutOverride,
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (NpcConfiguration)other;

                return cast != null &&
                    cast.Flags == this.Flags &&
                    cast.MagickaOffset == this.MagickaOffset &&
                    cast.StaminaOffset == this.StaminaOffset &&
                    cast.Level == this.Level &&
                    cast.CalcMinLevel == this.CalcMinLevel &&
                    cast.CalcMaxLevel == this.CalcMaxLevel &&
                    cast.SpeedMultiplier == this.SpeedMultiplier &&
                    cast.DispositionBase == this.DispositionBase &&
                    cast.TemplateFlags == this.TemplateFlags &&
                    cast.HealthOffset == this.HealthOffset &&
                    cast.BleedoutOverride == this.BleedoutOverride;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return string.Format("ACBS {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}", Flags, MagickaOffset, StaminaOffset, Level, CalcMinLevel, CalcMaxLevel, SpeedMultiplier, DispositionBase, TemplateFlags, HealthOffset, BleedoutOverride);
            }
        }

        public sealed class FactionField: Field
        {
            public uint Faction;
            public byte Rank;
            public byte[] Unused;

            public override Field CopyField()
            {
                return new FactionField()
                {
                    Faction = this.Faction,
                    Rank = this.Rank,
                    Unused = this.Unused
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (FactionField)other;
                return cast != null &&
                    cast.Faction != Faction &&
                    cast.Rank != Rank &&
                    cast.Unused != Unused;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield return Faction;
            }

            public override string ToString()
            {
                throw new NotImplementedException();
            }

            internal override void ReadField(RecordReader reader)
            {
                Faction = reader.ReadReference(FormKindSet.FactOnly);
                Rank = reader.ReadByte();
                Unused = new byte[3];
                reader.Read(Unused, 0, 3);
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.WriteReference(Faction, FormKindSet.FactOnly);
                writer.Write(Rank);
                writer.Write(Unused);
            }
        }

        public sealed class PerkField: Field
        {
            public uint Perk;
            public byte Rank;
            public byte[] Unused;

            public override Field CopyField()
            {
                return new PerkField()
                {
                    Perk = this.Perk,
                    Rank = this.Rank,
                    Unused = this.Unused
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (PerkField)other;
                return cast != null &&
                    cast.Perk != Perk &&
                    cast.Rank != Rank &&
                    cast.Unused != Unused;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield return Perk;
            }

            public override string ToString()
            {
                throw new NotImplementedException();
            }

            internal override void ReadField(RecordReader reader)
            {
                Perk = reader.ReadReference(FormKindSet.PerkOnly);
                Rank = reader.ReadByte();
                Unused = new byte[3];
                reader.Read(Unused, 0, 3);
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.WriteReference(Perk, FormKindSet.PerkOnly);
                writer.Write(Rank);
                writer.Write(Unused);
            }
        }
    }
}
