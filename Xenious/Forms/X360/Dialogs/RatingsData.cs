using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Xbox360;
using Xbox360.XEX;

namespace Xenious.Forms.Dialogs
{
    partial class RatingsData : Form
    {
        XeGame_Ratings ratings;
        public bool delete_ratings = false;
        public bool add_ratings = false;
        public bool has_ratings_header = false;

        public XeGame_Ratings get_ratings()
        {
            return ratings;
        }
        public void set_ratings_data(XeGame_Ratings r)
        {
            ratings = r;
        }
        public RatingsData()
        {
            InitializeComponent();
        }

        private void init_ratings_data()
        {
            #region ESRB.
            comboBox1.Items.Clear();
            comboBox1.Items.Add("EC");
            comboBox1.Items.Add("E");
            comboBox1.Items.Add("E10");
            comboBox1.Items.Add("T");
            comboBox1.Items.Add("M");
            comboBox1.Items.Add("AO");
            comboBox1.Items.Add("Unrated");

            switch (ratings.esrb)
            {
                case XeRating_esrb.eC:
                    comboBox1.SelectedIndex = 0;
                    break;
                case XeRating_esrb.E:
                    comboBox1.SelectedIndex = 1;
                    break;
                case XeRating_esrb.E10:
                    comboBox1.SelectedIndex = 2;
                    break;
                case XeRating_esrb.T:
                    comboBox1.SelectedIndex = 3;
                    break;
                case XeRating_esrb.M:
                    comboBox1.SelectedIndex = 4;
                    break;
                case XeRating_esrb.AO:
                    comboBox1.SelectedIndex = 5;
                    break;
                case XeRating_esrb.UNRATED:
                    comboBox1.SelectedIndex = 6;
                    break;
            }
            #endregion
            #region PEGI
            comboBox2.Items.Clear();
            comboBox2.Items.Add("3+");
            comboBox2.Items.Add("7+");
            comboBox2.Items.Add("12+");
            comboBox2.Items.Add("16+");
            comboBox2.Items.Add("18+");
            comboBox2.Items.Add("Unrated");

            switch (ratings.pegi)
            {
                case XeRating_pegi.ThreePLUS:
                    comboBox2.SelectedIndex = 0;
                    break;
                case XeRating_pegi.SevenPLUS:
                    comboBox2.SelectedIndex = 1;
                    break;
                case XeRating_pegi.TwelvePLUS:
                    comboBox2.SelectedIndex = 2;
                    break;
                case XeRating_pegi.SixteenPLUS:
                    comboBox2.SelectedIndex = 3;
                    break;
                case XeRating_pegi.EighteenPLUS:
                    comboBox2.SelectedIndex = 4;
                    break;
                case XeRating_pegi.UNRATED:
                    comboBox2.SelectedIndex = 5;
                    break;
            }
            #endregion
            #region PEGI Finland
            comboBox3.Items.Clear();
            comboBox3.Items.Add("3+");
            comboBox3.Items.Add("7+");
            comboBox3.Items.Add("11+");
            comboBox3.Items.Add("15+");
            comboBox3.Items.Add("18+");
            comboBox3.Items.Add("Unrated");

            switch (ratings.pegifi)
            {
                case XeRating_pegi_fi.ThreePLUS:
                    comboBox3.SelectedIndex = 0;
                    break;
                case XeRating_pegi_fi.SevenPLUS:
                    comboBox3.SelectedIndex = 1;
                    break;
                case XeRating_pegi_fi.ElevenPLUS:
                    comboBox3.SelectedIndex = 2;
                    break;
                case XeRating_pegi_fi.FifteenPLUS:
                    comboBox3.SelectedIndex = 3;
                    break;
                case XeRating_pegi_fi.EighteenPLUS:
                    comboBox3.SelectedIndex = 4;
                    break;
                case XeRating_pegi_fi.UNRATED:
                    comboBox3.SelectedIndex = 5;
                    break;

            }
            #endregion
            #region PEGI Portugal
            comboBox4.Items.Clear();
            comboBox4.Items.Add("4+");
            comboBox4.Items.Add("6+");
            comboBox4.Items.Add("12+");
            comboBox4.Items.Add("16+");
            comboBox4.Items.Add("18+");
            comboBox4.Items.Add("Unrated");

            switch (ratings.pegipt)
            {
                case XeRating_pegi_pt.FourPLUS:
                    comboBox4.SelectedIndex = 0;
                    break;
                case XeRating_pegi_pt.SixPLUS:
                    comboBox4.SelectedIndex = 1;
                    break;
                case XeRating_pegi_pt.TwelvePLUS:
                    comboBox4.SelectedIndex = 2;
                    break;
                case XeRating_pegi_pt.SixteenPLUS:
                    comboBox4.SelectedIndex = 3;
                    break;
                case XeRating_pegi_pt.EighteenPLUS:
                    comboBox4.SelectedIndex = 4;
                    break;
                case XeRating_pegi_pt.UNRATED:
                    comboBox4.SelectedIndex = 5;
                    break;
            }
            #endregion
            #region BBFC
            comboBox5.Items.Clear();
            comboBox5.Items.Add("Universal");
            comboBox5.Items.Add("PG");
            comboBox5.Items.Add("3+");
            comboBox5.Items.Add("7+");
            comboBox5.Items.Add("12+");
            comboBox5.Items.Add("15+");
            comboBox5.Items.Add("16+");
            comboBox5.Items.Add("18+");
            comboBox5.Items.Add("Unrated");

            switch (ratings.bbfc)
            {
                case XeRating_bbfc.UNIVERSAL:
                    comboBox5.SelectedIndex = 0;
                    break;
                case XeRating_bbfc.PG:
                    comboBox5.SelectedIndex = 1;
                    break;
                case XeRating_bbfc.ThreePLUS:
                    comboBox5.SelectedIndex = 2;
                    break;
                case XeRating_bbfc.SevenPLUS:
                    comboBox5.SelectedIndex = 3;
                    break;
                case XeRating_bbfc.TwelvePLUS:
                    comboBox5.SelectedIndex = 4;
                    break;
                case XeRating_bbfc.FifteenPLUS:
                    comboBox5.SelectedIndex = 5;
                    break;
                case XeRating_bbfc.SixteenPLUS:
                    comboBox5.SelectedIndex = 6;
                    break;
                case XeRating_bbfc.EighteenPLUS:
                    comboBox5.SelectedIndex = 7;
                    break;
                case XeRating_bbfc.UNRATED:
                    comboBox5.SelectedIndex = 8;
                    break;
            }
            #endregion
            #region CERO
            comboBox6.Items.Clear();
            comboBox6.Items.Add("A");
            comboBox6.Items.Add("B");
            comboBox6.Items.Add("C");
            comboBox6.Items.Add("D");
            comboBox6.Items.Add("Z");
            comboBox6.Items.Add("Unrated");

            switch (ratings.cero)
            {
                case XeRating_cero.A:
                    comboBox6.SelectedIndex = 0;
                    break;
                case XeRating_cero.B:
                    comboBox6.SelectedIndex = 1;
                    break;
                case XeRating_cero.C:
                    comboBox6.SelectedIndex = 2;
                    break;
                case XeRating_cero.D:
                    comboBox6.SelectedIndex = 3;
                    break;
                case XeRating_cero.Z:
                    comboBox6.SelectedIndex = 4;
                    break;
                case XeRating_cero.UNRATED:
                    comboBox6.SelectedIndex = 5;
                    break;
            }
            #endregion
            #region USK
            comboBox7.Items.Clear();
            comboBox7.Items.Add("All");
            comboBox7.Items.Add("6+");
            comboBox7.Items.Add("12+");
            comboBox7.Items.Add("16+");
            comboBox7.Items.Add("18+");
            comboBox7.Items.Add("Unrated");

            switch (ratings.usk)
            {
                case XeRating_usk.ALL:
                    comboBox7.SelectedIndex = 0;
                    break;
                case XeRating_usk.SixPLUS:
                    comboBox7.SelectedIndex = 1;
                    break;
                case XeRating_usk.TwelvePLUS:
                    comboBox7.SelectedIndex = 2;
                    break;
                case XeRating_usk.SixteenPLUS:
                    comboBox7.SelectedIndex = 3;
                    break;
                case XeRating_usk.EighteenPLUS:
                    comboBox7.SelectedIndex = 4;
                    break;
                case XeRating_usk.UNRATED:
                    comboBox7.SelectedIndex = 5;
                    break;
            }
            #endregion
            #region OFLC-AUS
            comboBox8.Items.Clear();
            comboBox8.Items.Add("G");
            comboBox8.Items.Add("PG");
            comboBox8.Items.Add("M");
            comboBox8.Items.Add("MA15+");
            comboBox8.Items.Add("Unrated");

            switch (ratings.oflcau)
            {
                case XeRating_oflc_au.G:
                    comboBox8.SelectedIndex = 0;
                    break;
                case XeRating_oflc_au.PG:
                    comboBox8.SelectedIndex = 1;
                    break;
                case XeRating_oflc_au.M:
                    comboBox8.SelectedIndex = 2;
                    break;
                case XeRating_oflc_au.MA15_PLUS:
                    comboBox8.SelectedIndex = 3;
                    break;
                case XeRating_oflc_au.UNRATED:
                    comboBox8.SelectedIndex = 4;
                    break;
            }
            #endregion
            #region OFLC-NZ
            comboBox9.Items.Clear();
            comboBox9.Items.Add("G");
            comboBox9.Items.Add("PG");
            comboBox9.Items.Add("M");
            comboBox9.Items.Add("MA15+");
            comboBox9.Items.Add("Unrated");

            switch (ratings.oflcnz)
            {
                case XeRating_oflc_nz.G:
                    comboBox9.SelectedIndex = 0;
                    break;
                case XeRating_oflc_nz.PG:
                    comboBox9.SelectedIndex = 1;
                    break;
                case XeRating_oflc_nz.M:
                    comboBox9.SelectedIndex = 2;
                    break;
                case XeRating_oflc_nz.MA15_PLUS:
                    comboBox9.SelectedIndex = 3;
                    break;
                case XeRating_oflc_nz.UNRATED:
                    comboBox9.SelectedIndex = 4;
                    break;
            }
            #endregion
            #region KMRB
            comboBox10.Items.Clear();
            comboBox10.Items.Add("All");
            comboBox10.Items.Add("12+");
            comboBox10.Items.Add("15+");
            comboBox10.Items.Add("18+");
            comboBox10.Items.Add("Unrated");

            switch (ratings.kmrb)
            {
                case XeRating_kmrb.ALL:
                    comboBox10.SelectedIndex = 0;
                    break;
                case XeRating_kmrb.TwelvePLUS:
                    comboBox10.SelectedIndex = 1;
                    break;
                case XeRating_kmrb.FifteenPLUS:
                    comboBox10.SelectedIndex = 2;
                    break;
                case XeRating_kmrb.EighteenPLUS:
                    comboBox10.SelectedIndex = 3;
                    break;
                case XeRating_kmrb.UNRATED:
                    comboBox10.SelectedIndex = 4;
                    break;
            }
            #endregion
            #region Brazil
            comboBox11.Items.Clear();
            comboBox11.Items.Add("All");
            comboBox11.Items.Add("12+");
            comboBox11.Items.Add("14+");
            comboBox11.Items.Add("16+");
            comboBox11.Items.Add("18+");
            comboBox11.Items.Add("Unknown");
            comboBox11.Items.Add("Unrated");

            switch (ratings.brazil)
            {
                case XeRating_brazil.ALL:
                    comboBox11.SelectedIndex = 0;
                    break;
                case XeRating_brazil.TwelvePLUS:
                    comboBox11.SelectedIndex = 1;
                    break;
                case XeRating_brazil.ForteenPLUS:
                    comboBox11.SelectedIndex = 2;
                    break;
                case XeRating_brazil.SixteenPLUS:
                    comboBox11.SelectedIndex = 3;
                    break;
                case XeRating_brazil.EighteenPLUS:
                    comboBox11.SelectedIndex = 4;
                    break;
                case XeRating_brazil.Unknown:
                    comboBox11.SelectedIndex = 5;
                    break;
                case XeRating_brazil.UNRATED:
                    comboBox11.SelectedIndex = 6;
                    break;
            }
            #endregion
            #region FPB

            comboBox12.Items.Clear();
            comboBox12.Items.Add("All");
            comboBox12.Items.Add("PG");
            comboBox12.Items.Add("10+");
            comboBox12.Items.Add("13+");
            comboBox12.Items.Add("16+");
            comboBox12.Items.Add("18+");
            comboBox12.Items.Add("Unrated");

            switch (ratings.fpb)
            {
                case XeRating_fpb.ALL:
                    comboBox12.SelectedIndex = 0;
                    break;
                case XeRating_fpb.PG:
                    comboBox12.SelectedIndex = 1;
                    break;
                case XeRating_fpb.TenPLUS:
                    comboBox12.SelectedIndex = 2;
                    break;
                case XeRating_fpb.ThirteenPLUS:
                    comboBox12.SelectedIndex = 3;
                    break;
                case XeRating_fpb.SixteenPLUS:
                    comboBox12.SelectedIndex = 4;
                    break;
                case XeRating_fpb.EighteenPLUS:
                    comboBox12.SelectedIndex = 5;
                    break;
                case XeRating_fpb.UNRATED:
                    comboBox12.SelectedIndex = 6;
                    break;
            }

            #endregion
            richTextBox2.Text = BitConverter.ToString(ratings.reserved).ToUpper().Replace("-", "");
        }

        private void RatingsData_Load(object sender, EventArgs e)
        {
            if (has_ratings_header == true)
            {
                init_ratings_data();
                button1.Text = "Delete Ratings Data";
            }
            else
            {
                button1.Text = "Add Ratings Data";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Todo fix.
            if (has_ratings_header == true)
            {
                comboBox1.Enabled = true;
                comboBox1.SelectedIndex = 6;
                comboBox2.Enabled = true;
                comboBox2.SelectedIndex = 5;
                comboBox3.Enabled = true;
                comboBox3.SelectedIndex = 5;
                comboBox4.Enabled = true;
                comboBox4.SelectedIndex = 5;
                comboBox5.Enabled = true;
                comboBox5.SelectedIndex = 8;
                comboBox6.Enabled = true;
                comboBox6.SelectedIndex = 5;
                comboBox7.Enabled = true;
                comboBox7.SelectedIndex = 5;
                comboBox8.Enabled = true;
                comboBox8.SelectedIndex = 4;
                comboBox9.Enabled = true;
                comboBox9.SelectedIndex = 4;
                comboBox10.Enabled = true;
                comboBox10.SelectedIndex = 4;
                comboBox11.Enabled = true;
                comboBox11.SelectedIndex = 5;
                comboBox12.Enabled = true;
                comboBox12.SelectedIndex = 6;
                richTextBox2.ReadOnly = false;
                richTextBox2.Text = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";
                button1.Text = "Delete Ratings Data";
                delete_ratings = true;
                add_ratings = false;
            }
            else
            {
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox5.Enabled = false;
                comboBox6.Enabled = false;
                comboBox7.Enabled = false;
                comboBox8.Enabled = false;
                comboBox9.Enabled = false;
                comboBox10.Enabled = false;
                comboBox11.Enabled = false;
                comboBox12.Enabled = false;
                richTextBox2.Text = "";
                richTextBox2.ReadOnly = true;
                button1.Text = "Add Ratings Data";
                add_ratings = true;
                delete_ratings = false;
                // This will be addded when closed.
                has_ratings_header = false;
            }
        }
        private void RatingsData_FormClosing(object sender, FormClosingEventArgs e)
        {
            #region ESRB.
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    ratings.esrb = XeRating_esrb.eC;
                    break;
                case 1:
                    ratings.esrb = XeRating_esrb.E;
                    break;
                case 2:
                    ratings.esrb = XeRating_esrb.E10;
                    break;
                case 3:
                    ratings.esrb = XeRating_esrb.T;
                    break;
                case 4:
                    ratings.esrb = XeRating_esrb.M;
                    break;
                case 5:
                    ratings.esrb = XeRating_esrb.AO;
                    break;
                case 6:
                    ratings.esrb = XeRating_esrb.UNRATED;
                    break;
            }
            #endregion
            #region PEGI
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    ratings.pegi = XeRating_pegi.ThreePLUS;
                    break;
                case 1:
                    ratings.pegi = XeRating_pegi.SevenPLUS;
                    break;
                case 2:
                    ratings.pegi = XeRating_pegi.TwelvePLUS;
                    break;
                case 3:
                    ratings.pegi = XeRating_pegi.SixteenPLUS;
                    break;
                case 4:
                    ratings.pegi = XeRating_pegi.EighteenPLUS;
                    break;
                case 5:
                    ratings.pegi = XeRating_pegi.UNRATED;
                    break;
            }
            #endregion
            #region PEGI Finland
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    ratings.pegifi = XeRating_pegi_fi.ThreePLUS;
                    break;
                case 1:
                    ratings.pegifi = XeRating_pegi_fi.SevenPLUS;
                    break;
                case 2:
                    ratings.pegifi = XeRating_pegi_fi.ElevenPLUS;
                    break;
                case 3:
                    ratings.pegifi = XeRating_pegi_fi.FifteenPLUS;
                    break;
                case 4:
                    ratings.pegifi = XeRating_pegi_fi.EighteenPLUS;
                    break;
                case 5:
                    ratings.pegifi = XeRating_pegi_fi.UNRATED;
                    break;

            }
            #endregion
            #region PEGI Portugal
            switch (comboBox4.SelectedIndex)
            {
                case 0:
                    ratings.pegipt = XeRating_pegi_pt.FourPLUS;
                    break;
                case 1:
                    ratings.pegipt = XeRating_pegi_pt.SixPLUS;
                    break;
                case 2:
                    ratings.pegipt = XeRating_pegi_pt.TwelvePLUS;
                    break;
                case 3:
                    ratings.pegipt = XeRating_pegi_pt.SixteenPLUS;
                    break;
                case 4:
                    ratings.pegipt = XeRating_pegi_pt.EighteenPLUS;
                    break;
                case 5:
                    ratings.pegipt = XeRating_pegi_pt.UNRATED;
                    break;
            }
            #endregion
            #region BBFC
            switch (comboBox5.SelectedIndex)
            {
                case 0:
                    ratings.bbfc = XeRating_bbfc.UNIVERSAL;
                    break;
                case 1:
                    ratings.bbfc = XeRating_bbfc.PG;
                    break;
                case 2:
                    ratings.bbfc = XeRating_bbfc.ThreePLUS;
                    break;
                case 3:
                    ratings.bbfc = XeRating_bbfc.SevenPLUS;
                    break;
                case 4:
                    ratings.bbfc = XeRating_bbfc.TwelvePLUS;
                    break;
                case 5:
                    ratings.bbfc = XeRating_bbfc.FifteenPLUS;
                    break;
                case 6:
                    ratings.bbfc = XeRating_bbfc.SixteenPLUS;
                    break;
                case 7:
                    ratings.bbfc = XeRating_bbfc.EighteenPLUS;
                    break;
                case 8:
                    ratings.bbfc = XeRating_bbfc.UNRATED;
                    break;
            }
            #endregion
            #region CERO
            switch (comboBox6.SelectedIndex)
            {
                case 0:
                    ratings.cero = XeRating_cero.A;
                    break;
                case 1:
                    ratings.cero = XeRating_cero.B;
                    break;
                case 2:
                    ratings.cero = XeRating_cero.C;
                    break;
                case 3:
                    ratings.cero = XeRating_cero.D;
                    break;
                case 4:
                    ratings.cero = XeRating_cero.Z;
                    break;
                case 5:
                    ratings.cero = XeRating_cero.UNRATED;
                    break;
            }
            #endregion
            #region USK
            switch (comboBox7.SelectedIndex)
            {
                case 0:
                    ratings.usk = XeRating_usk.ALL;
                    break;
                case 1:
                    ratings.usk = XeRating_usk.SixPLUS;
                    break;
                case 2:
                    ratings.usk = XeRating_usk.TwelvePLUS;
                    break;
                case 3:
                    ratings.usk = XeRating_usk.SixteenPLUS;
                    break;
                case 4:
                    ratings.usk = XeRating_usk.EighteenPLUS;
                    break;
                case 5:
                    ratings.usk = XeRating_usk.UNRATED;
                    break;
            }
            #endregion
            #region OFLC-AUS
            switch (comboBox8.SelectedIndex)
            {
                case 0:
                    ratings.oflcau = XeRating_oflc_au.G;
                    break;
                case 1:
                    ratings.oflcau = XeRating_oflc_au.PG;
                    break;
                case 2:
                    ratings.oflcau = XeRating_oflc_au.M;
                    break;
                case 3:
                    ratings.oflcau = XeRating_oflc_au.MA15_PLUS;
                    break;
                case 4:
                    ratings.oflcau = XeRating_oflc_au.UNRATED;
                    break;
            }
            #endregion
            #region OFLC-NZ
            switch (comboBox9.SelectedIndex)
            {
                case 0:
                    ratings.oflcnz = XeRating_oflc_nz.G;
                    break;
                case 1:
                    ratings.oflcnz = XeRating_oflc_nz.PG;
                    break;
                case 2:
                    ratings.oflcnz = XeRating_oflc_nz.M;
                    break;
                case 3:
                    ratings.oflcnz = XeRating_oflc_nz.MA15_PLUS;
                    break;
                case 4:
                    ratings.oflcnz = XeRating_oflc_nz.UNRATED;
                    break;
            }
            #endregion
            #region KMRB
            switch (comboBox10.SelectedIndex)
            {
                case 0:
                    ratings.kmrb = XeRating_kmrb.ALL;
                    break;
                case 1:
                    ratings.kmrb = XeRating_kmrb.TwelvePLUS;
                    break;
                case 2:
                    ratings.kmrb = XeRating_kmrb.FifteenPLUS;
                    break;
                case 3:
                    ratings.kmrb = XeRating_kmrb.EighteenPLUS;
                    break;
                case 4:
                    ratings.kmrb = XeRating_kmrb.UNRATED;
                    break;
            }
            #endregion
            #region Brazil
            switch (comboBox11.SelectedIndex)
            {
                case 0:
                    ratings.brazil = XeRating_brazil.ALL;
                    break;
                case 1:
                    ratings.brazil = XeRating_brazil.TwelvePLUS;
                    break;
                case 2:
                    ratings.brazil = XeRating_brazil.ForteenPLUS;
                    break;
                case 3:
                    ratings.brazil = XeRating_brazil.SixteenPLUS;
                    break;
                case 4:
                    ratings.brazil = XeRating_brazil.EighteenPLUS;
                    break;
                case 5:
                    ratings.brazil = XeRating_brazil.Unknown;
                    break;
                case 6:
                    ratings.brazil = XeRating_brazil.UNRATED;
                    break;
            }
            #endregion
            #region FPB
            switch (comboBox12.SelectedIndex)
            {
                case 0:
                    ratings.fpb = XeRating_fpb.ALL;
                    break;
                case 1:
                    ratings.fpb = XeRating_fpb.PG;
                    break;
                case 2:
                    ratings.fpb = XeRating_fpb.TenPLUS;
                    break;
                case 3:
                    ratings.fpb = XeRating_fpb.ThirteenPLUS;
                    break;
                case 4:
                    ratings.fpb = XeRating_fpb.SixteenPLUS;
                    break;
                case 5:
                    ratings.fpb = XeRating_fpb.EighteenPLUS;
                    break;
                case 6:
                    ratings.fpb = XeRating_fpb.UNRATED;
                    break;
            }

            #endregion
        }
    }
}
