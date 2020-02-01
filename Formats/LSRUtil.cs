﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SRtoolbox
{
    public class LSRUtil
    {
        public enum Theme
        {
           None = -1,
           Jungle = 0,
           Ice = 1,
           Desert = 2,
           City = 3
        }

        public enum TrackType
        {
            Multiplayer = 0,
            Singleplayer = 1,
        }

        public enum TrackTime
        {
            Day = 0,
            Night = 1
        }

        [Flags]
        public enum Compatibility
        {
            Full = 0, // Compatible
            Overlap = 1, // Track pieces overlap [TODO]
            EightSixteen = 2, // Not 8x8 or 16x16
            NonSquare = 4, // Not square track size
            TooHigh = 8, // Pieces are over 3 height
            ThemeMismatch = 16, // Some piece have different theme
            CustomHeader = 32, // LEGO MOTO header mismatch
        }

        public class TrackCoord : IEquatable<TrackCoord>
        {
            public int X { get; set; }
            public int Y { get; set; }

            public TrackCoord(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public override string ToString()
            {
                return ToString(false);
            }

            public string ToString(bool size)
            {
                return string.Format("{0}x{1}", size ? this.X + 1 : this.X, size ? this.Y + 1 : this.Y);
            }

            public override bool Equals(object value)
            {
                if (value == null) return false;

                TrackCoord coord = value as TrackCoord;

                return !ReferenceEquals(null, coord) &&
                       Equals(X, coord.X) &&
                       Equals(Y, coord.Y);
            }

            public bool Equals(TrackCoord other)
            {
                return !ReferenceEquals(null, other) &&
                       Equals(X, other.X) &&
                       Equals(Y, other.Y);
            }

            public override int GetHashCode()
            {
                var hashCode = 1861411795;
                hashCode = hashCode * -1521134295 + X.GetHashCode();
                hashCode = hashCode * -1521134295 + Y.GetHashCode();
                return hashCode;
            }

            // This is gross -Y

            public static bool operator ==(TrackCoord x, TrackCoord y)
            {
                return x.X == y.X && x.Y == y.Y;
            }

            public static bool operator !=(TrackCoord x, TrackCoord y)
            {
                return !(x == y);
            }
        }

        public static int[][] Pieces = {
            /* Jungle */ new int[] { 0x65, 0x61, 0x62, 0x63, 0x64, 0x66, 0x67, 0x68, 0x69, 0x6B, 0x6C, 0x6F, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x83, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92, 0x93, 0x94, 0x66, 0x97, 0x98, 0x9C, 0x9D, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAB, 0xAC, 0xAD, 0xAE, 0xAF, 0xBA, 0xBB, 0xBC, 0xBD, 0xBE, 0xBF, 0xC0 },
            /* Ice */    new int[] { 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4E, 0x4F, 0x50, 0x51, 0x53, 0x54, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6B, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7E, 0x7F, 0x80, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8 },
            /* Desert */ new int[] { 0x1D, 0x19, 0x1A, 0x1B, 0x1C, 0x1E, 0x1F, 0x20, 0x21, 0x23, 0x24, 0x27, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3B, 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4E, 0x4F, 0x50, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78 },
            /* City */   new int[] { 0x30, 0x31, 0x32, 0x33, 0x34, 0x36, 0x37, 0x38, 0x39, 0x3B, 0x3C, 0x3F, 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x53, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60, 0x61, 0x62, 0x63, 0x64, 0x66, 0x67, 0x68, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90 }
        };

        public int GetPiece(Theme theme, int nid)
        {
            return Pieces[(int)theme][nid];
        }

        public string GetThemeName(Theme theme)
        {
            return theme.ToString();
        }

        public static Theme TRK_GetTheme(byte theme)
        {
            Theme t = Theme.None;

            if (theme == 0x3B) t = Theme.Jungle;
            else if (theme == 0x3F) t = Theme.Ice;
            else if (theme == 0x47) t = Theme.Desert;
            else if (theme == 0x43) t = Theme.City;
            return t;
        }

        public static int GetNid(int id, Theme theme)
        {
            int ret = 0;

            if (theme != Theme.None)
            {
                int[] tb = Pieces[(int)theme];

                foreach (int i in tb)
                {
                    if (i == id) break;
                    ret += 1;
                }
            }

            return ret;
        }

        public static TrackPiece ParsePiece(byte[][] data)
        {
            int height = (int)BitConverter.ToSingle(data[0],0);
            if (height == -1) height = 0;
            else height /= 8;

            byte[] piece = data[1];

            TrackPiece ret = new TrackPiece()
            {
                id = piece[0],
                rotation = piece[2],
                theme = TRK_GetTheme(piece[1]),
                height = height
            };

            ret.nid = GetNid(ret.id, ret.theme);

            return ret;
        }

        public static Compatibility CheckCompat(TrackFile track)
        {
            Compatibility compat = Compatibility.Full;

            string lego = (track as TRKFile).legoHeader;
            if (lego != "LEGO MOTO\0\0\0" && lego != string.Empty) compat |= Compatibility.CustomHeader;

            if (track.size.X != track.size.Y) compat |= Compatibility.NonSquare;
            if (track.size != new TrackCoord(7, 7) && track.size != new TrackCoord(15, 15)) { MessageBox.Show("Track: "+track.size.ToString()); compat |= Compatibility.EightSixteen; }

            foreach (var piece in track.pieces)
            {
                if (piece.height > 3)
                {
                    //MessageBox.Show("Height: " + piece.height);
                    compat |= Compatibility.TooHigh;
                }
                if (piece.theme != track.theme && piece.theme != Theme.None)
                {
                    //MessageBox.Show("Piece: " + piece.theme.ToString(), "Track: " + track.theme.ToString());
                    compat |= Compatibility.ThemeMismatch;
                }
            }

            return compat;
        }
    }
}