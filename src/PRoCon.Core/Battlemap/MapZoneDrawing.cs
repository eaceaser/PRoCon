/*  Copyright 2010 Geoffrey 'Phogue' Green

    http://www.phogue.net
 
    This file is part of PRoCon Frostbite.

    PRoCon Frostbite is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    PRoCon Frostbite is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with PRoCon Frostbite.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PRoCon.Core.Battlemap {
    using Core.Remote;

    [Serializable]
    public class MapZoneDrawing : MapZone {

        public delegate void TagsEditedHandler(MapZoneDrawing sender);
        public event TagsEditedHandler TagsEdited;

        public MapZoneDrawing(string strUid, string strLevelFileName, string strTagList, Point3D[] a_pntZonePolygon, bool blInclusive)
            : base(strUid, strLevelFileName, strTagList, a_pntZonePolygon, blInclusive) {
            this.Tags.TagsEdited += new ZoneTagList.TagsEditedHandler(Tags_TagsEdited);
        }

        public GraphicsPath ZoneGraphicsPath {
            get {
                GraphicsPath gpReturn = new GraphicsPath();
                PointF[] pntPolygon = new PointF[this.ZonePolygon.Length];
                for (int i = 0; i < this.ZonePolygon.Length; i++) {
                    pntPolygon[i] = new PointF(this.ZonePolygon[i].X, this.ZonePolygon[i].Y);
                }
                gpReturn.AddPolygon(pntPolygon);
                gpReturn.CloseFigure();

                return gpReturn;
            }
        }

        // Returns a percentage of the ErrorArea circle trespassing on the zone.
        // If anyone knows calculus better than me I'd welcome you to clean up this function =)
        public float TrespassArea(Point3D pntLocation, float flErrorRadius) {

            float flReturnPercentage = 0.0F;
            float flErrorArea = (float)(flErrorRadius * flErrorRadius * Math.PI);

            GraphicsPath gpLocationError = new GraphicsPath();
            gpLocationError.AddEllipse(new RectangleF(pntLocation.X - flErrorRadius, pntLocation.Y - flErrorRadius, flErrorRadius * 2, flErrorRadius * 2));
            gpLocationError.CloseAllFigures();

            Region regZone = new Region(this.ZoneGraphicsPath);
            regZone.Intersect(gpLocationError);
            RectangleF[] a_recScans = regZone.GetRegionScans(new Matrix());
            Rectangle recIntersection = new Rectangle(int.MaxValue, int.MaxValue, 0, 0);

            int iPixelCount = 0;

            if (a_recScans.Length > 0) {

                for (int i = 0; i < a_recScans.Length; i++) {
                    recIntersection.X = a_recScans[i].X < recIntersection.X ? (int)a_recScans[i].X : recIntersection.X;
                    recIntersection.Y = a_recScans[i].Y < recIntersection.Y ? (int)a_recScans[i].Y : recIntersection.Y;

                    recIntersection.Width = a_recScans[i].Right > recIntersection.Right ? (int)a_recScans[i].Right - recIntersection.X : recIntersection.Width;
                    recIntersection.Height = a_recScans[i].Bottom > recIntersection.Bottom ? (int)a_recScans[i].Bottom - recIntersection.Y : recIntersection.Height;
                }

                //recIntersection = this.RecFtoRec(regZone.GetBounds(this.CreateGraphics()));
                Point pntVisible = new Point(recIntersection.X, recIntersection.Y);

                for (pntVisible.X = recIntersection.X; pntVisible.X <= recIntersection.Right; pntVisible.X++) {
                    for (pntVisible.Y = recIntersection.Y; pntVisible.Y <= recIntersection.Bottom; pntVisible.Y++) {
                        if (regZone.IsVisible(pntVisible) == true) {
                            iPixelCount++;
                        }
                    }
                }
            }

            flReturnPercentage = (float)iPixelCount / flErrorArea;

            // Accounts for low error when using this method. (98.4% should be 100%)
            // but using regZone.GetRegionScans is slightly lossy.
            if (flReturnPercentage > 0.0F) {
                flReturnPercentage = (float)Math.Min(1.0F, flReturnPercentage + 0.02);
            }

            return flReturnPercentage;
        }

        private void Tags_TagsEdited(ZoneTagList sender) {
            if (this.TagsEdited != null) {
                FrostbiteConnection.RaiseEvent(this.TagsEdited.GetInvocationList(), this);
            }
        }

    }
}
