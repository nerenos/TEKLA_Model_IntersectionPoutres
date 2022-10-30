using System;
using System.Collections;
using System.Collections.Generic;

using System.Windows.Forms;
using Tekla.Structures.Model;
using TSG3D = Tekla.Structures.Geometry3d;
using Tekla.Structures.Model.UI;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model.Operations;



namespace Tekla.Technology.Akit.UserScript
{
    public class Script
    {
		public static double LenghtPoints(Point myPoint1, Point myPoint2)
		{
			return Math.Sqrt(Math.Pow(myPoint2.X-myPoint1.X, 2)+Math.Pow(myPoint2.Y-myPoint1.Y, 2)+Math.Pow(myPoint2.Z-myPoint1.Z, 2));
		}
		
        public static void Run(Tekla.Technology.Akit.IScript akit)
        {
            try
            {
				while (true)
				{
					
					DialogResult result;
					Model myModel = new Model();
					myModel.CommitChanges();
					Picker myPicker = new Picker();
					Beam myBeam1 = myPicker.PickObject(0, "Selectionnez la 1er poutre") as Beam;
					Beam myBeam2 = myPicker.PickObject(0, "Selectionnez la 2eme poutre") as Beam;
					double Tvar = 0;
					bool T = false;
					double Svar = 0;
					bool S = false;
					Point interPoint;
					
					Point P1 = myBeam1.StartPoint;
					Point myPoint12 = myBeam1.EndPoint;
					Point P2 = myBeam2.StartPoint;
					Point myPoint22 = myBeam2.EndPoint;
					
					Vector V1 = new Vector(Math.Round(myPoint12.X-P1.X,2), Math.Round(myPoint12.Y-P1.Y,2), Math.Round(myPoint12.Z-P1.Z,2));
					Vector V2 = new Vector(Math.Round(myPoint22.X-P2.X,2), Math.Round(myPoint22.Y-P2.Y,2), Math.Round(myPoint22.Z-P2.Z,2));
					
					if (V2.X == 0 && V1.X != 0) {Tvar = (P2.X-P1.X)/V1.X; T = true;}
					if (V1.X == 0 && V2.X != 0) {Svar = (P1.X-P2.X)/V2.X; S = true;}
					if (V2.Y == 0 && V1.Y != 0) {Tvar = (P2.Y-P1.Y)/V1.Y; T = true;}
					if (V1.Y == 0 && V2.Y != 0) {Svar = (P1.Y-P2.Y)/V2.Y; S = true;}
					if (V2.Z == 0 && V1.Z != 0) {Tvar = (P2.Z-P1.Z)/V1.Z; T = true;}
					if (V1.Z == 0 && V2.Z != 0) {Svar = (P1.Z-P2.Z)/V2.Z; S = true;}
					
					
					
					//double Tvar = (V2.X*(P1.Y-P2.Y)+V2.Y*(P2.X-P1.X))/((V2.Y*V1.X)-(V2.X*V1.Y));
					System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("fr-FR");
					string Ttest = Convert.ToString(Tvar, culture);
					if (T) {					
						interPoint = new Point(V1.X*Tvar+P1.X, V1.Y*Tvar+P1.Y, V1.Z*Tvar+P1.Z);
					}else{
						interPoint = new Point(V2.X*Svar+P2.X, V2.Y*Svar+P2.Y, V2.Z*Svar+P2.Z);
					}
					
					//result = MessageBox.Show("\n P1(" + P1.X + ", " + P1.Y + ", " + P1.Z + ")" + "\n P2(" + P2.X + ", " + P2.Y + ", " + P2.Z + ")" + "\n V1(" + V1.X + ", " + V1.Y + ", " + V1.Z + ")" + "\n V2(" + V2.X + ", " + V2.Y + ", " + V2.Z + ")" + "\n x:" + interPoint.X + "\n y:" + interPoint.Y + "\n Z:" + interPoint.Z + "\n T:" + Tvar + "\n S:" + Svar, "test", MessageBoxButtons.YesNo);
									
					if (LenghtPoints(interPoint, P1) < LenghtPoints(interPoint, myPoint12)) {
						myBeam1.StartPoint = interPoint;
					}else{
						myBeam1.EndPoint = interPoint;
					}
					myBeam1.Modify();
					
					if (LenghtPoints(interPoint, P2) < LenghtPoints(interPoint, myPoint22)) {
						myBeam2.StartPoint = interPoint;
					}else{
						myBeam2.EndPoint = interPoint;
					}
					myBeam2.Modify();
					
					//ControlPoint myPoint = new ControlPoint(interPoint);
					//myPoint.Insert();
								
				}
			}
			catch
            {
                Operation.DisplayPrompt("Commande interrompue par l'utilisateur");
            }
		}
	}
}