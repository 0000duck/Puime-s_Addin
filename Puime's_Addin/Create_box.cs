﻿using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using System;
using System.Drawing;

namespace Puime_s_Addin
{
    public partial class Create_box : ToolWindow
    {
        public Create_box()
        {
            InitializeComponent();

            int tw_width = UIEnvironment.Windows["ObjectBrowser"].Control.Size.Width - 30;
            this.Caption = "Create ABB Box";
            
            this.PreferredSize = new Size(tw_width, 330);
            UIEnvironment.Windows.AddDocked(this, System.Windows.Forms.DockStyle.Top, UIEnvironment.Windows["ObjectBrowser"] as ToolWindow);
            //Logger.AddMessage(new LogMessage(this.Control.ToString(), "Puime's Add-in"));
        }

        private void size_TextChanged(object sender, EventArgs e)
        {
            var bl = (length_textbox.Value != 0) && (width_textbox.Value != 0) && (height_textbox.Value != 0);
            btn_create.Enabled = bl;
        }

        private void btn_clear_clicked(object sender, EventArgs e)
        {
            this.Close();
            new Create_box();
        }
        private void btn_close_clicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_create_clicked(object sender, EventArgs e)
        {
            #region Create ABB_Box
            Project.UndoContext.BeginUndoStep("BodyCreateSolids");

            try
            {
                Station station = Project.ActiveProject as Station;

                //
                // Create a part to contain the bodies.
                #region BodyCreateSolidsStep1
                Part p = new Part();
                p.Name = "ABB_Box";
                station.GraphicComponents.Add(p);
                #endregion

                //
                // Create a solid box.
                #region Create Box
                //Vector3 vect_position = new Vector3(pos_control.Value.x, pos_control.Value.y, pos_control.Value.z);
                Vector3 vect_position = new Vector3(0, 0, 0); //uses 0,0,0 as origin to later transform the position to the pos_control values,
                                                              //so the part origin is allways in the corner of the box.
                Vector3 vect_orientation = new Vector3(orientation_control.Value.x, orientation_control.Value.y, orientation_control.Value.z);
                Matrix4 matrix_origo = new Matrix4(vect_position, vect_orientation);
                Vector3 size = new Vector3(length_textbox.Value / 1000, width_textbox.Value / 1000, height_textbox.Value / 1000);

                Body b1 = Body.CreateSolidBox(matrix_origo, size);
                b1.Name = "Box";
                p.Bodies.Add(b1);
                #endregion

                // Transform the position of the part to the values of the pos_control values. So the part origin is allways in the corner of the box.
                p.Transform.X = pos_control.Value.x;
                p.Transform.X = pos_control.Value.y;
                p.Transform.Z = pos_control.Value.z;

                //
                // Get the faces from the box.
                Face myFace0 = b1.Shells[0].Faces[0]; // z +
                Face myFace1 = b1.Shells[0].Faces[1]; // z -
                Face myFace2 = b1.Shells[0].Faces[2]; // y -
                Face myFace3 = b1.Shells[0].Faces[3]; // x -
                Face myFace4 = b1.Shells[0].Faces[4]; // y +
                Face myFace5 = b1.Shells[0].Faces[5]; // x +

                //
                // Make sure the faces are visible.
                myFace0.Visible = true;
                myFace1.Visible = true;
                myFace2.Visible = true;
                myFace3.Visible = true;
                myFace4.Visible = true;
                myFace5.Visible = true;

                //
                // Set the material for each face of the box
                Bitmap bmp0 = new Bitmap(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ABB Industrial IT\\Robotics IT\\Puime's Addin\\Textures\\top.jpg");
                Bitmap bmp1 = new Bitmap(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ABB Industrial IT\\Robotics IT\\Puime's Addin\\Textures\\bottom.jpg");
                Bitmap bmp2 = new Bitmap(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ABB Industrial IT\\Robotics IT\\Puime's Addin\\Textures\\long_side.jpg");
                Bitmap bmp3 = new Bitmap(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ABB Industrial IT\\Robotics IT\\Puime's Addin\\Textures\\short_side2.jpg");
                Bitmap bmp4 = new Bitmap(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ABB Industrial IT\\Robotics IT\\Puime's Addin\\Textures\\long_side2.jpg");
                Bitmap bmp5 = new Bitmap(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\ABB Industrial IT\\Robotics IT\\Puime's Addin\\Textures\\short_side.jpg");
                Texture texture0 = new Texture(bmp0);
                Texture texture1 = new Texture(bmp1);
                Texture texture2 = new Texture(bmp2);
                Texture texture3 = new Texture(bmp3);
                Texture texture4 = new Texture(bmp4);
                Texture texture5 = new Texture(bmp5);
                Material material0 = new Material(texture0);
                Material material1 = new Material(texture1);
                Material material2 = new Material(texture2);
                Material material3 = new Material(texture3);
                Material material4 = new Material(texture4);
                Material material5 = new Material(texture5);
                myFace0.SetMaterial(material0);
                myFace1.SetMaterial(material1);
                myFace2.SetMaterial(material2);
                myFace3.SetMaterial(material3);
                myFace4.SetMaterial(material4);
                myFace5.SetMaterial(material5);


                //
                // Reset the ToolWindow
                this.Close();
                new Create_box();

            }

            catch
            {
                Project.UndoContext.CancelUndoStep(CancelUndoStepType.Rollback);
                throw;
            }
            finally
            {
                Project.UndoContext.EndUndoStep();
            }
            #endregion
        } //private void btn_create_clicked(object sender, EventArgs e)
    } //private void btn_create_clicked(object sender, EventArgs e)
}
