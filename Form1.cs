using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace StudiuIndividual
{
    public partial class studiu : Form
    {
        string connection;
        public studiu()
        {
            InitializeComponent();
            //Face marimea aplicatie egala cu marimea ecranului
            WindowState = FormWindowState.Maximized;
            //Stabileste pozitia panoului pnlConnection pentru a se afla in mijocul aplicatiei
            pnlConnection.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlConnection.Width / 2,
                Screen.PrimaryScreen.WorkingArea.Height / 2 - pnlConnection.Height / 2);
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPass.Text;
            //Creaza stringul de conexiune folosind datele din variabile login si password
            connection = "Data Source=Server_Name;Initial Catalog=DataBase;Persist Security Info=True;User ID=" + login + ";Password=" + password;
            using (SqlConnection con = new SqlConnection(connection))
            {
                try
                {
                    con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        pnlConnection.Visible = false;
                        //Seteaza proprietatile Location, Anchor, Width Visible a panoului pnlMenu
                        pnlMenu.Location = new Point(0, 0);
                        pnlMenu.Anchor = AnchorStyles.None;
                        pnlMenu.Width = this.Width;
                        menuStrip1.Margin = new Padding(Screen.PrimaryScreen.WorkingArea.Width / 2 - menuStrip1.Width / 2, 0, 0, 0);
                        pnlMenu.Visible = true;

                        //Verifica login daca este Angajat si ascunde toate optiunile de adaugare, modificare si stergere
                        if(login == "Angajat")
                        {
                            angajatToolStripMenuItem1.Visible = false;
                            companieDeIngredienteToolStripMenuItem1.Visible = false;
                            filialaToolStripMenuItem1.Visible = false;
                            functieToolStripMenuItem1.Visible = false;

                            angajatToolStripMenuItem2.Visible = false;
                            companieDeIngredienteToolStripMenuItem2.Visible = false;
                            filialaToolStripMenuItem2.Visible = false;

                            stergereToolStripMenuItem.Visible = false;
                        }
                        else
                        {
                            angajatToolStripMenuItem1.Visible = true;
                            companieDeIngredienteToolStripMenuItem1.Visible = true;
                            filialaToolStripMenuItem1.Visible = true;
                            functieToolStripMenuItem1.Visible = true;

                            angajatToolStripMenuItem2.Visible = true;
                            companieDeIngredienteToolStripMenuItem2.Visible = true;
                            filialaToolStripMenuItem2.Visible = true;

                            stergereToolStripMenuItem.Visible = true;
                        }
                    }
                    con.Close();
                }
                catch(Exception)
                {
                    lblIncorrect.Text = "Date de logare gresite";
                }
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            //Ascunde toate panourile
            foreach (Control c in this.Controls)
            {
                if (c is Panel)
                {
                    c.Visible = false;
                }
            }
            //Face panoul pnlConnection visibil si sterge datele din textBox-urile txtLogin si txtPass
            pnlConnection.Visible = true;
            txtLogin.Text = string.Empty;
            txtPass.Text = string.Empty;
        }

        private void hidePanels()
        {//Ascunde toate panourile inafara de pnlMenu
            foreach (Control c in this.Controls)
            {
                if (c.Name == "pnlMenu") continue;
                if (c is Panel)
                {
                    c.Visible = false;
                }
            }
        }

        private void angajatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlEmployeeSelect.Visible = true;
            pnlEmployeeSelect.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlEmployeeSelect.Width / 2, 24);
            pnlEmployeeSelect.BringToFront();
        }

        private void produsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlProductSelect.Visible = true;
            pnlProductSelect.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlProductSelect.Width / 2, 24);
            pnlProductSelect.BringToFront();
            createProductComboBox();
        }

        private void ingredienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlIngredientSelect.Visible = true;
            pnlIngredientSelect.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlIngredientSelect.Width / 2, 24);
            pnlIngredientSelect.BringToFront();
            createIngComboBox();
        }

        private void filialaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlFacilitySelect.Visible = true;
            pnlFacilitySelect.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlFacilitySelect.Width / 2, 24);
            pnlFacilitySelect.BringToFront();
            facilityLabel();
        }

        private void functieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlWorkSelect.Visible = true;
            pnlWorkSelect.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlWorkSelect.Width / 2, 24);
            pnlWorkSelect.BringToFront();
            workLabel();
        }

        private void masiniDeTransportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlTransportSelect.Visible = true;
            pnlTransportSelect.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlTransportSelect.Width / 2, 24);
            pnlTransportSelect.BringToFront();
            createTransportBox();
        }

        private void companieDeIngredienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlIngCompanySelect.Visible = true;
            pnlIngCompanySelect.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlIngCompanySelect.Width / 2, 24);
            pnlIngCompanySelect.BringToFront();
            createIngCompanyBox();
        }

        private void btnSearchEmployee_Click(object sender, EventArgs e)
        {
            if (txtIDNP.Text.Length == 13)
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();
                    string query = "SELECT Top 1 ID FROM Angajat WHERE IDNP = " + txtIDNP.Text;
                    lblEmployeeResult.Text += txtIDNP.Text;
                    SqlCommand cmd = new SqlCommand(query, con);
                    if (cmd.ExecuteScalar() != null)
                    {
                        lblEmployeeResult.Text = "IDNP: " + txtIDNP.Text;

                        query = "SELECT Nume + ' ' + Prenume FROM Angajat WHERE IDNP = " + txtIDNP.Text;
                        cmd = new SqlCommand(query, con);
                        lblEmployeeResult.Text += "\n\nNume: " + Convert.ToString(cmd.ExecuteScalar());

                        query = "SELECT Gen FROM Angajat WHERE IDNP = " + txtIDNP.Text;
                        cmd = new SqlCommand(query, con);
                        lblEmployeeResult.Text += "\n\nSex: " + Convert.ToString(cmd.ExecuteScalar());

                        query = "SELECT DataNastere FROM Angajat WHERE IDNP = " + txtIDNP.Text;
                        cmd = new SqlCommand(query, con);
                        lblEmployeeResult.Text += "\n\nData de Nastere:" + Convert.ToString(cmd.ExecuteScalar());

                        query = "SELECT Telefon FROM Angajat WHERE IDNP = " + txtIDNP.Text;
                        cmd = new SqlCommand(query, con);
                        lblEmployeeResult.Text += "\n\nNumar de Telefon: " + Convert.ToString(cmd.ExecuteScalar());

                        query = "SELECT Adresa FROM Angajat WHERE IDNP = " + txtIDNP.Text;
                        cmd = new SqlCommand(query, con);
                        lblEmployeeResult.Text += "\n\nAdresa: " + Convert.ToString(cmd.ExecuteScalar());

                        query = "SELECT Email FROM Angajat WHERE IDNP = " + txtIDNP.Text;
                        cmd = new SqlCommand(query, con);
                        lblEmployeeResult.Text += "\n\nEmail: " + Convert.ToString(cmd.ExecuteScalar());

                        query = "SELECT Salariu FROM Angajat WHERE IDNP = " + txtIDNP.Text;
                        cmd = new SqlCommand(query, con);
                        lblEmployeeResult.Text += "\n\nSalariu: " + Convert.ToString(cmd.ExecuteScalar()) + " lei";

                        query = "SELECT Denumire FROM Functie WHERE ID = (SELECT ID_Functie FROM Angajat WHERE IDNP = " + txtIDNP.Text + ")";
                        cmd = new SqlCommand(query, con);
                        lblEmployeeResult.Text += "\n\nFunctie: " + Convert.ToString(cmd.ExecuteScalar());

                        query = "SELECT Denumire FROM Filiala WHERE ID = (SELECT ID_Filiala FROM Angajat WHERE IDNP = " + txtIDNP.Text + ")";
                        cmd = new SqlCommand(query, con);
                        lblEmployeeResult.Text += "\n\nFiliala: " + Convert.ToString(cmd.ExecuteScalar());

                        query = "SELECT Adresa FROM Filiala WHERE ID = (SELECT ID_Filiala FROM Angajat WHERE IDNP = " + txtIDNP.Text + ")";
                        cmd = new SqlCommand(query, con);
                        lblEmployeeResult.Text += "\n\n   Adresa: " + Convert.ToString(cmd.ExecuteScalar());

                        query = "SELECT Telefon FROM Filiala WHERE ID = (SELECT ID_Filiala FROM Angajat WHERE IDNP = " + txtIDNP.Text + ")";
                        cmd = new SqlCommand(query, con);
                        lblEmployeeResult.Text += "\n\n   Telefon: " + Convert.ToString(cmd.ExecuteScalar());
                    }
                }
                pnlEmployeeSelect.Size = new Size(319, lblEmployeeResult.Height + 110);
            }
        }

        private void createIngComboBox()
        {
            using(SqlConnection con = new SqlConnection(connection))
            {
                con.Open ();
                string query = "SELECT Denumire FROM Ingrediente";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (pnlIngredientSelect.Visible)
                    {
                        boxIngredient.Items.Add(reader[0]);
                    }
                    if(pnlIngredientUpdate.Visible)
                    {
                        boxIngredientUpdate.Items.Add(reader[0]);
                    }
                    if (pnlIngredientDelete.Visible)
                    {
                        boxIngredientDelete.Items.Add(reader[0]);
                    }
                }
                con.Close ();
            }
        }

        private void boxIngredient_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT * FROM getProduse ('" + boxIngredient.Text + "')";
                SqlCommand cmd = new SqlCommand(query, con);

                string result = Convert.ToString(cmd.ExecuteScalar());
                string[] productList = result.Split(',');
                lblIngResult.Text = "Produse: ";
                for (int i = 0; i < productList.Length; i++)
                {
                    lblIngResult.Text += productList[i] + ",";
                    if (i >= 2 || i % 2 == 0)
                    {
                        lblIngResult.Text += "\n";
                    }
                }
                if(productList.Length%2==0) lblIngResult.Text+= "\n";

                query = "SELECT Pret FROM Ingrediente WHERE Denumire = '" + boxIngredient.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblIngResult.Text += "\n\nPret: " + String.Format("{0,4:F2}", Convert.ToDouble(result)) + " lei\n";

                query = "SELECT TimpValabilitate FROM Ingrediente WHERE Denumire = '" + boxIngredient.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblIngResult.Text += "\nTimp de Valabilitate: " + result + "\n";

                query = "SELECT DataExpirarii FROM Ingrediente WHERE Denumire = '" + boxIngredient.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblIngResult.Text += "\nData Expirarii: " + result;

                con.Close();
            }
            pnlIngredientSelect.Size = new Size(lblIngResult.Width + 100, lblIngResult.Height + 110);
        }

        private void createProductComboBox()
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT Denumire FROM Produs";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (pnlProductSelect.Visible)
                    {
                        boxProduct.Items.Add(reader[0]);
                    }
                    if (pnlProductUpdate.Visible)
                    {
                        boxProductUpdate.Items.Add(reader[0]);
                    }
                    if (pnlProductDelete.Visible)
                    {
                        boxProductDelete.Items.Add(reader[0]);
                    }
                }
                con.Close();
            }
        }

        private void boxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT * FROM getIngrediente ('" + boxProduct.Text + "')";
                SqlCommand cmd = new SqlCommand(query, con);
                string result = Convert.ToString(cmd.ExecuteScalar());

                string[] productList = result.Split(',');
                lblProductResult.Text = "Ingrediente: ";
                for (int i = 0; i < productList.Length; i++)
                {
                    lblProductResult.Text += productList[i] + ",";
                    if (i >= 2 && i % 2 == 0)
                    {
                        lblProductResult.Text += "\n";
                    }
                }

                query = "SELECT Pret FROM Produs WHERE Denumire = '" + boxProduct.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblProductResult.Text += "\n\nPret: " + String.Format("{0,4:F2}", Convert.ToDouble(result)) + " lei\n";

                query = "SELECT TimpValabilitate FROM Produs WHERE Denumire = '" + boxProduct.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblProductResult.Text += "\nTimp de Valabilitate: " + result + "\n";

                query = "SELECT DataExpirarii FROM Produs WHERE Denumire = '" + boxProduct.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblProductResult.Text += "\nData Expirarii: " + result;

                con.Close();
            }
            pnlProductSelect.Size = new Size(lblProductResult.Width + 110, lblProductResult.Height + 110);
        }

        private void facilityLabel()
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT Denumire, Adresa, Telefon, Email FROM Filiala WHERE ID = 'fil001'";
                SqlCommand cmd = new SqlCommand(query, con); 
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lblFacilityResult1.Text += "Denumire: " + reader.GetString(0) + "\n\nAdresa: " + reader.GetString(1) + "\n\nTelefon: " + reader.GetInt32(2) + "\n\nEmail: " + reader.GetString(3);
                }
                reader.Close();

                query = "SELECT Denumire, Adresa, Telefon, Email FROM Filiala WHERE ID = 'fil002'";
                cmd = new SqlCommand(query, con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lblFacilityResult2.Text += "Denumire: " + reader.GetString(0) + "\n\nAdresa: " + reader.GetString(1) + "\n\nTelefon: " + reader.GetInt32(2) + "\n\nEmail: " + reader.GetString(3);
                }
                reader.Close();

                pnlFacilitySelect.Size = new Size(lblFacilityResult1.Width * 2 + 180, lblFacilityResult1.Height * 2);

                con.Close();
            }
        }

        private void workLabel()
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT STRING_AGG(Denumire, ',') AS Denumiri FROM Functie";
                SqlCommand cmd = new SqlCommand(query, con);
                string result = Convert.ToString(cmd.ExecuteScalar());

                string[] workList = result.Split(',');
                for (int i = 0; i < workList.Length; i++)
                {
                    lblWorkResult.Text += workList[i] + "\n";
                    query = "SELECT * FROM getAngajati ('fun00" + Convert.ToString(i + 1) + "')";
                    if (i >= 9) query = "SELECT * FROM getAngajati ('fun0" + Convert.ToString(i + 1) + "')";
                    cmd = new SqlCommand (query, con);
                    string result1 = Convert.ToString(cmd.ExecuteScalar());
                    lblWorkResult.Text += "   " + result1 + "\n";
                }
                con.Close();
            }
            pnlWorkSelect.Size = new Size(lblWorkResult.Width + 100, lblWorkResult.Height + 80);
         }

        private void createTransportBox()
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT ID FROM MasiniTransport";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (pnlTransportInsert.Visible)
                    {
                        boxTransport.Items.Add(reader[0]);
                    }
                    if (pnlTransportUpdate.Visible)
                    {
                        boxTransportUpdate.Items.Add(reader[0]);
                    }
                    if (pnlTransportDelete.Visible)
                    {
                        boxTransportDelete.Items.Add(reader[0]);
                    }
                }
                con.Close();
            }
        }

        private void boxTransport_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT Tip FROM MasiniTransport WHERE ID = '" + boxTransport.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                string result = Convert.ToString(cmd.ExecuteScalar());
                lblTransportResult.Text = "Tip: " + result + "\n";

                query = "SELECT CantitateMax FROM MasiniTransport WHERE ID = '" + boxTransport.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblTransportResult.Text += "\nCantitate max: " + result + "\n";

                query = "SELECT Nume + ' ' + Prenume AS NumeAngajat FROM Angajat WHERE ID = (SELECT ID_Sofer FROM MasiniTransport WHERE ID = '" + boxTransport.Text + "')";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblTransportResult.Text += "\nSofer: " + result + "\n";

                con.Close();
            }
            pnlTransportSelect.Size = new Size(lblTransportResult.Width + 100, lblTransportResult.Height + 120);
        }

        private void createIngCompanyBox()
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT Denumire FROM CompanieIngrediente";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (pnlIngCompanyInsert.Visible)
                    {
                        boxIngCompany.Items.Add(reader[0]);
                    }
                    if (pnlIngCompanyUpdate.Visible)
                    {
                        boxIngCompanyUpdate.Items.Add(reader[0]);
                    }
                    if (pnlIngCompanyDelete.Visible)
                    {
                        boxIngCompanyDelete.Items.Add(reader[0]);
                    }
                }
                con.Close();
            }
        }

        private void boxIngCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT Adresa FROM CompanieIngrediente WHERE Denumire = '" + boxIngCompany.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                string result = Convert.ToString(cmd.ExecuteScalar());
                lblIngCompanyResult.Text = "Adresa: " + result;

                query = "SELECT Telefon FROM CompanieIngrediente WHERE Denumire = '" + boxIngCompany.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblIngCompanyResult.Text += "\n\nTelefon: " + result;

                query = "SELECT Email FROM CompanieIngrediente WHERE Denumire = '" + boxIngCompany.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblIngCompanyResult.Text += "\n\nEmail: " + result;

                query = "SELECT ID FROM CompanieIngrediente WHERE Denumire = '" + boxIngCompany.Text + "'";
                cmd = new SqlCommand(query, con);
                string id = Convert.ToString(cmd.ExecuteScalar());

                query = "SELECT STRING_AGG(Denumire, ', ') AS IngredienteDenumiri FROM Ingrediente WHERE ID IN (SELECT ID_Ingrediente FROM IngredienteLinkComp WHERE ID_CompanieIngrediente = '" + id + "')";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblIngCompanyResult.Text += "\n\nIngrediente: " + result;

                con.Close();
            }
            pnlIngCompanySelect.Size = new Size(lblIngCompanyResult.Width + 60, lblIngCompanyResult.Height + 100);
        }
        private void angajatToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlEmployeeInsert.Size = new Size(411, 340);
            pnlEmployeeInsert.Visible = true;
            pnlEmployeeInsert.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlEmployeeInsert.Width / 2, 24);
            pnlEmployeeInsert.BringToFront();
            createEmployeeBoxes();
        }

        private void produsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlProductInsert.Size = new Size(653, 314);
            pnlProductInsert.Visible = true;
            pnlProductInsert.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlProductInsert.Width / 2, 24);
            pnlProductInsert.BringToFront();
            createNumBox();
        }

        private void ingredienteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlIngredientInsert.Size = new Size(292, 149);
            pnlIngredientInsert.Visible = true;
            pnlIngredientInsert.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlIngredientInsert.Width / 2, 24);
            pnlIngredientInsert.BringToFront();
        }

        private void filialaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlFacilityInsert.Size = new Size(254, 149);
            pnlFacilityInsert.Visible = true;
            pnlFacilityInsert.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlFacilityInsert.Width / 2, 24);
            pnlFacilityInsert.BringToFront();
        }

        private void functieToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlWorkInsert.Size = new Size(200, 100);
            pnlWorkInsert.Visible = true;
            pnlWorkInsert.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlWorkInsert.Width / 2, 24);
            pnlWorkInsert.BringToFront();
        }

        private void masiniDeTransportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlTransportInsert.Size = new Size(245, 121);
            pnlTransportInsert.Visible = true;
            pnlTransportInsert.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlTransportInsert.Width / 2, 24);
            pnlTransportInsert.BringToFront();
        }

        private void companieDeIngredienteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlIngCompanyInsert.Size = new Size(669, 156);
            pnlIngCompanyInsert.Visible = true;
            pnlIngCompanyInsert.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlIngCompanyInsert.Width / 2, 24);
            pnlIngCompanyInsert.BringToFront();
            createBoxNum2();
        }

        private void createEmployeeBoxes()
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT Denumire FROM Functie";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (pnlEmployeeInsert.Visible)
                    {
                        boxEmployeeWork.Items.Add(reader[0]);
                    }
                    if(pnlEmployeeUpdate.Visible)
                    {
                        boxWorkUpdate.Items.Add(reader[0]);
                    }
                }
                reader.Close();

                query = "SELECT Denumire FROM Filiala";
                cmd = new SqlCommand(query, con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (pnlEmployeeInsert.Visible)
                    {
                        boxEmployeeFacility.Items.Add(reader[0]);
                    }
                    if (pnlEmployeeUpdate.Visible)
                    {
                        boxFacultyUpdate.Items.Add(reader[0]);
                    }
                }
                reader.Close();

                con.Close();
            }
        }
        private void rBtnOther_CheckedChanged(object sender, EventArgs e)
        {
            txtOtherSex.Visible = true;
            txtOtherSex.Enabled = true;
        }

        private void rBtnMasc_CheckedChanged(object sender, EventArgs e)
        {
            txtOtherSex.Visible = false;
            txtOtherSex.Enabled = false;
        }

        private void rBtnFem_CheckedChanged(object sender, EventArgs e)
        {
            txtOtherSex.Visible = false;
            txtOtherSex.Enabled = false;
        }

        private void btnEmployeeSubmit_Click(object sender, EventArgs e)
        {
            if (txtEmployeeIDNP.Text != "" || txtEmployeeName.Text != "" || txtEmployeePhone.Text != "")
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();
                    string query = "SELECT COUNT(ID) FROM Angajat";
                    SqlCommand cmd = new SqlCommand(query, con);

                    string id;
                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0 && Convert.ToInt32(cmd.ExecuteScalar()) < 10)
                    {
                        id = "a00" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }
                    else if (Convert.ToInt32(cmd.ExecuteScalar()) >= 10 && Convert.ToInt32(cmd.ExecuteScalar()) < 100)
                    {
                        id = "a0" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }
                    else
                    {
                        id = "a" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }

                    query = "SELECT ID FROM Functie WHERE Denumire = '" + boxEmployeeWork.Text + "'";
                    cmd = new SqlCommand(query, con);
                    string workID = Convert.ToString(cmd.ExecuteScalar());

                    query = "SELECT ID FROM Filiala WHERE Denumire = '" + boxEmployeeFacility.Text + "'";
                    cmd = new SqlCommand(query, con);
                    string facilityID = Convert.ToString(cmd.ExecuteScalar());

                    string gender = "NaN";
                    if (rBtnMasc.Checked)
                    {
                        gender = rBtnMasc.Text;
                    }
                    else if (rBtnFem.Checked)
                    {
                        gender = rBtnFem.Text;
                    }
                    else if (rBtnOther.Checked)
                    {
                        gender = txtOtherSex.Text;
                    }

                    query = "EXECUTE insertAngajat '" + id + "', " + txtEmployeeIDNP.Text + ", '" + txtEmployeeName.Text + "', '"
                        + txtEmployeeSurname.Text + "', '" + gender + "', '" + txtEmployeeBirth.Text + "', " + txtEmployeePhone.Text + ", '"
                        + txtEmployeeAdress.Text + "' , '" + txtEmployeeEmail.Text + "', " + txtEmployeeSalary.Text + ", '" + workID + "', '" + facilityID + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    txtEmployeeIDNP.Text = string.Empty;
                    txtEmployeeName.Text = string.Empty;
                    txtEmployeeSurname.Text = string.Empty;
                    txtEmployeeBirth.Text = string.Empty;
                    txtEmployeePhone.Text = string.Empty;
                    txtEmployeeAdress.Text = string.Empty;
                    txtEmployeeEmail.Text = string.Empty;
                    txtEmployeeSalary.Text = string.Empty;
                    boxEmployeeWork.Text = string.Empty;
                    boxEmployeeFacility.Text = string.Empty;
                    txtOtherSex.Text = string.Empty;
                    rBtnMasc.Checked = false;
                    rBtnFem.Checked = false;
                    rBtnOther.Checked = false;
                }
            }
        }

        private void createNumBox()
        {
            boxNum.Items.Clear();
            boxNum3.Items.Clear();
            object[] arr = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            boxNum.Items.AddRange(arr);
            boxNum3.Items.AddRange(arr);
        }

        private void boxNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(boxNum.SelectedIndex >= 0)
            {
                groupIng1.Visible = true;
                pnlProductInsert.Size = new Size(btnProductSubmit.Width + btnProductSubmit.Location.X, groupIng1.Location.Y + groupIng1.Height + 50);
                groupIng2.Visible = false;
                groupIng3.Visible = false;
                groupIng4.Visible = false;
                groupIng5.Visible = false;
                groupIng6.Visible = false;
                groupIng7.Visible = false;
                groupIng8.Visible = false;
                groupIng9.Visible = false;
                groupIng10.Visible = false;
                groupIng11.Visible = false;
                groupIng12.Visible = false;
                groupIng13.Visible = false;
            }
            if (boxNum.SelectedIndex >= 1)
            {
                groupIng2.Visible = true;
                pnlProductInsert.Width += groupIng2.Width + 10;
            }
            if (boxNum.SelectedIndex >= 2)
            {
                groupIng3.Visible = true;
                pnlProductInsert.Height = groupIng3.Location.Y + groupIng3.Height + 50;
            }
            if (boxNum.SelectedIndex >= 3)
            {
                groupIng4.Visible = true;
            }
            if (boxNum.SelectedIndex >= 4)
            {
                groupIng5.Visible = true;
                pnlProductInsert.Height = groupIng5.Location.Y + groupIng5.Height + 50;
            }
            if (boxNum.SelectedIndex >= 5)
            {
                groupIng6.Visible = true;
            }
            if (boxNum.SelectedIndex >= 6)
            {
                groupIng7.Visible = true;
                pnlProductInsert.Height = groupIng7.Location.Y + groupIng7.Height + 50;
            }
            if (boxNum.SelectedIndex >= 7)
            {
                groupIng8.Visible = true;
            }
            if (boxNum.SelectedIndex >= 8)
            {
                groupIng9.Visible = true;
                pnlProductInsert.Height = groupIng9.Location.Y + groupIng9.Height + 50;
            }
            if (boxNum.SelectedIndex >= 9)
            {
                groupIng10.Visible = true;
            }
            if (boxNum.SelectedIndex >= 10)
            {
                groupIng11.Visible = true;
                pnlProductInsert.Height = groupIng11.Location.Y + groupIng11.Height + 50;
            }
            if (boxNum.SelectedIndex >= 11)
            {
                groupIng12.Visible = true;
            }
            if (boxNum.SelectedIndex >= 12)
            {
                groupIng13.Visible = true;
                pnlProductInsert.Height = groupIng13.Location.Y + groupIng13.Height + 50;
            }
            if (boxNum.SelectedIndex == 13)
            {
                groupIng14.Visible = true;
            }
        }

        private void btnProductSubmit_Click(object sender, EventArgs e)
        {
            if (txtProductName.Text != "" || txtProductCost.Text != "" || txtProductDate.Text != "")
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();

                    string query = "SELECT COUNT(ID) FROM Produs";
                    SqlCommand cmd = new SqlCommand(query, con);

                    string id;
                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0 && Convert.ToInt32(cmd.ExecuteScalar()) < 10)
                    {
                        id = "p00" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }
                    else if (Convert.ToInt32(cmd.ExecuteScalar()) >= 10 && Convert.ToInt32(cmd.ExecuteScalar()) < 100)
                    {
                        id = "p0" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }
                    else
                    {
                        id = "p" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }

                    query = "EXECUTE insertProduse '" + id + "', '" + txtProductName.Text + "', " + txtProductCost.Text + ", " + Convert.ToInt32(txtProductDate.Text);
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();

                    string idIng;
                    //1st ing
                    if (!groupIng1.Visible)
                    {
                        if (txtIngProdName1.Text != "" || txtIngProdQuantity1.Text != "" || txtIngProdMeasure1.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdName1.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantity1.Text) + ",'" + txtIngProdMeasure1.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //2nd ing
                    if (!groupIng2.Visible)
                    {
                        if (txtIngProdName2.Text != "" || txtIngProdQuantity2.Text != "" || txtIngProdMeasure2.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdName2.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantity2.Text) + ",'" + txtIngProdMeasure2.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //3rd ing
                    if (!groupIng3.Visible)
                    {
                        if (txtIngProdName3.Text != "" || txtIngProdQuantity3.Text != "" || txtIngProdMeasure3.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdName3.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantity3.Text) + ",'" + txtIngProdMeasure3.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //4th ing
                    if (!groupIng4.Visible)
                    {
                        if (txtIngProdName4.Text != "" || txtIngProdQuantity4.Text != "" || txtIngProdMeasure4.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdName4.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantity4.Text) + ",'" + txtIngProdMeasure4.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //5th ing
                    if (!groupIng5.Visible)
                    {
                        if (txtIngProdName5.Text != "" || txtIngProdQuantity5.Text != "" || txtIngProdMeasure5.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdName5.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantity5.Text) + ",'" + txtIngProdMeasure5.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //6th ing
                    if (!groupIng6.Visible)
                    {
                        if (txtIngProdName6.Text != "" || txtIngProdQuantity6.Text != "" || txtIngProdMeasure6.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdName6.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantity6.Text) + ",'" + txtIngProdMeasure6.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //7th ing
                    if (!groupIng7.Visible)
                    {
                        if (txtIngProdName7.Text != "" || txtIngProdQuantity7.Text != "" || txtIngProdMeasure7.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdName7.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantity7.Text) + ",'" + txtIngProdMeasure7.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //8th ing
                    if (!groupIng8.Visible)
                    {
                        if (txtIngProdName8.Text != "" || txtIngProdQuantity8.Text != "" || txtIngProdMeasure8.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdName8.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantity8.Text) + ",'" + txtIngProdMeasure8.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //9th ing
                    if (!groupIng9.Visible)
                    {
                        if (txtIngProdName9.Text != "" || txtIngProdQuantity9.Text != "" || txtIngProdMeasure9.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdName9.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantity9.Text) + ",'" + txtIngProdMeasure9.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //10th ing
                    if (!groupIng10.Visible)
                    {
                        if (txtIngProdName10.Text != "" || txtIngProdQuantity10.Text != "" || txtIngProdMeasure10.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdName10.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantity10.Text) + ",'" + txtIngProdMeasure10.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //11th ing
                    if (!groupIng11.Visible)
                    {
                        if (txtIngProdName11.Text != "" || txtIngProdQuantity11.Text != "" || txtIngProdMeasure11.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdName11.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantity11.Text) + ",'" + txtIngProdMeasure11.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //12th ing
                    if (!groupIng12.Visible)
                    {
                        if (txtIngProdName12.Text != "" || txtIngProdQuantity12.Text != "" || txtIngProdMeasure12.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdName12.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantity12.Text) + ",'" + txtIngProdMeasure12.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //13th ing
                    if (!groupIng13.Visible)
                    {
                        if (txtIngProdName13.Text != "" || txtIngProdQuantity13.Text != "" || txtIngProdMeasure13.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdName13.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantity13.Text) + ",'" + txtIngProdMeasure13.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //14th ing
                    if (!groupIng14.Visible)
                    {
                        if (txtIngProdName14.Text != "" || txtIngProdQuantity14.Text != "" || txtIngProdMeasure14.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdName14.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantity14.Text) + ",'" + txtIngProdMeasure14.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                txtProductCost.Text = string.Empty;
                txtProductName.Text = string.Empty;
                txtProductDate.Text = string.Empty;
                boxNum.Text = string.Empty;
                txtIngProdName1.Text = string.Empty;
                txtIngProdQuantity1.Text = string.Empty;
                txtIngProdMeasure1.Text = string.Empty;
                txtIngProdName2.Text = string.Empty;
                txtIngProdQuantity2.Text = string.Empty;
                txtIngProdMeasure2.Text = string.Empty;
                txtIngProdName3.Text = string.Empty;
                txtIngProdQuantity3.Text = string.Empty;
                txtIngProdMeasure3.Text = string.Empty;
                txtIngProdName4.Text = string.Empty;
                txtIngProdQuantity4.Text = string.Empty;
                txtIngProdMeasure4.Text = string.Empty;
                txtIngProdName5.Text = string.Empty;
                txtIngProdQuantity5.Text = string.Empty;
                txtIngProdMeasure5.Text = string.Empty;
                txtIngProdName6.Text = string.Empty;
                txtIngProdQuantity6.Text = string.Empty;
                txtIngProdMeasure6.Text = string.Empty;
                txtIngProdName7.Text = string.Empty;
                txtIngProdQuantity7.Text = string.Empty;
                txtIngProdMeasure7.Text = string.Empty;
                txtIngProdName8.Text = string.Empty;
                txtIngProdQuantity8.Text = string.Empty;
                txtIngProdMeasure8.Text = string.Empty;
                txtIngProdName9.Text = string.Empty;
                txtIngProdQuantity9.Text = string.Empty;
                txtIngProdMeasure9.Text = string.Empty;
                txtIngProdName10.Text = string.Empty;
                txtIngProdQuantity10.Text = string.Empty;
                txtIngProdMeasure10.Text = string.Empty;
                txtIngProdName11.Text = string.Empty;
                txtIngProdQuantity11.Text = string.Empty;
                txtIngProdMeasure11.Text = string.Empty;
                txtIngProdName12.Text = string.Empty;
                txtIngProdQuantity12.Text = string.Empty;
                txtIngProdMeasure12.Text = string.Empty;
                txtIngProdName13.Text = string.Empty;
                txtIngProdQuantity13.Text = string.Empty;
                txtIngProdMeasure13.Text = string.Empty;
                txtIngProdName14.Text = string.Empty;
                txtIngProdQuantity14.Text = string.Empty;
                txtIngProdMeasure14.Text = string.Empty;
            }
        }

        private void btnIngredientSubmit_Click(object sender, EventArgs e)
        {
            if (txtIngredientName.Text != "" || txtIngredientCost.Text != "" || txtIngredientQuantity.Text != "" || txtIngredientMeasure.Text != "" || txtIngredientDate.Text != "")
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();

                    string query = "SELECT COUNT(ID) FROM Ingrediente";
                    SqlCommand cmd = new SqlCommand(query, con);

                    string id;
                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0 && Convert.ToInt32(cmd.ExecuteScalar()) < 10)
                    {
                        id = "i00" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }
                    else if (Convert.ToInt32(cmd.ExecuteScalar()) >= 10 && Convert.ToInt32(cmd.ExecuteScalar()) < 100)
                    {
                        id = "i0" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }
                    else
                    {
                        id = "i" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }

                    query = "EXECUTE insertIngrediente '" + id + "', '" + txtIngredientName.Text + "', " + txtIngredientCost.Text + ", " + Convert.ToInt32(txtIngredientQuantity.Text) + ",'" + txtIngredientMeasure.Text + "', '" + txtIngredientDate.Text + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                txtIngredientCost.Text = string.Empty;
                txtIngredientMeasure.Text = string.Empty;
                txtIngredientName.Text = string.Empty;
                txtIngredientQuantity.Text = string.Empty;
                txtIngredientDate.Text = string.Empty;
            }
        }

        private void btnFacilitySubmit_Click(object sender, EventArgs e)
        {
            if (txtFacilityAdress.Text != "" || txtFacilityEmail.Text != "" || txtFacilityName.Text != "" || txtFacilityPhone.Text != "")
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();

                    string query = "SELECT COUNT(ID) FROM Filiala";
                    SqlCommand cmd = new SqlCommand(query, con);

                    string id;
                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0 && Convert.ToInt32(cmd.ExecuteScalar()) < 10)
                    {
                        id = "fil00" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }
                    else if (Convert.ToInt32(cmd.ExecuteScalar()) >= 10 && Convert.ToInt32(cmd.ExecuteScalar()) < 100)
                    {
                        id = "fil0" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }
                    else
                    {
                        id = "fil" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }

                    query = "EXECUTE insertFiliala '" + id + "', '" + txtFacilityName.Text + "', '" + txtFacilityAdress.Text + "', " + Convert.ToInt32(txtFacilityPhone.Text) + ", '" + txtFacilityEmail.Text + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                txtFacilityAdress.Text = string.Empty;
                txtFacilityEmail.Text = string.Empty;
                txtFacilityName.Text = string.Empty;
                txtFacilityPhone.Text = string.Empty;
            }
        }

        private void btnWorkSubmit_Click(object sender, EventArgs e)
        {
            if (txtWorkName.Text != "")
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();

                    string query = "SELECT COUNT(ID) FROM Functie";
                    SqlCommand cmd = new SqlCommand(query, con);

                    string id;
                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0 && Convert.ToInt32(cmd.ExecuteScalar()) < 10)
                    {
                        id = "fun00" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }
                    else if (Convert.ToInt32(cmd.ExecuteScalar()) >= 10 && Convert.ToInt32(cmd.ExecuteScalar()) < 100)
                    {
                        id = "fun0" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }
                    else
                    {
                        id = "fun" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }

                    query = "EXECUTE insertFunctie '" + id + "', '" + txtWorkName.Text + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                txtWorkName.Text = string.Empty;
            }
        }

        private void btnTransportSubmit_Click(object sender, EventArgs e)
        {
            if (txtWorkName.Text != "" || txtTransportMax.Text != "" || txtTransportDriver.Text != "")
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();

                    string query = "SELECT COUNT(ID) FROM MasiniTransport";
                    SqlCommand cmd = new SqlCommand(query, con);

                    string id;
                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0 && Convert.ToInt32(cmd.ExecuteScalar()) < 10)
                    {
                        id = "mt00" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }
                    else if (Convert.ToInt32(cmd.ExecuteScalar()) >= 10 && Convert.ToInt32(cmd.ExecuteScalar()) < 100)
                    {
                        id = "mt0" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }
                    else
                    {
                        id = "mt" + (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
                    }

                    string[] name = txtTransportDriver.Text.Split(' ');
                    query = "SELECT ID FROM Angajat WHERE Nume = '" + name[0] + "' AND '" + name[1] + "'";
                    cmd = new SqlCommand(query, con);
                    string idDriver = Convert.ToString(cmd.ExecuteScalar());

                    query = "EXECUTE insertTrasnport '" + id + "', '" + txtWorkName.Text + "' " + Convert.ToInt32(txtTransportMax.Text) + ",'" + idDriver + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                txtTransportDriver.Text = string.Empty;
                txtTransportMax.Text = string.Empty;
                txtTransportType.Text = string.Empty;
            }
        }

        private void createBoxNum2()
        {
            object[] arr = { 1, 2, 3, 4, 5, 6};
            if (pnlIngCompanyInsert.Visible)
            {
                boxNum2.Items.AddRange(arr);
            }
            if (pnlIngCompanyUpdate.Visible)
            {
                boxNum4.Items.AddRange(arr);
            }
        }

        private void boxNum2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (boxNum2.SelectedIndex >= 0)
            {
                groupIngComp1.Visible = true;
                pnlIngCompanyInsert.Width = Screen.PrimaryScreen.WorkingArea.Width;
                groupIngComp2.Visible = false;
                groupIngComp3.Visible = false;
                groupIngComp4.Visible = false;
                groupIngComp5.Visible = false;
                groupIngComp6.Visible = false;
            }
            if (boxNum2.SelectedIndex >= 1)
            {
                groupIngComp2.Visible = true;
                this.Width += groupIngComp2.Width + 10;
                pnlIngCompanyInsert.Width = Screen.PrimaryScreen.WorkingArea.Width;
            }
            if (boxNum2.SelectedIndex >= 2)
            {
                groupIngComp3.Visible = true;
                this.Height = groupIngComp3.Location.Y + groupIngComp3.Height + 50;
            }
            if (boxNum2.SelectedIndex >= 3)
            {
                groupIngComp4.Visible = true;
            }
            if (boxNum2.SelectedIndex >= 4)
            {
                groupIngComp5.Visible = true;
                this.Height = groupIngComp5.Location.Y + groupIngComp5.Height + 50;
                pnlIngCompanyInsert.Height = this.Height;
            }
            if (boxNum2.SelectedIndex >= 5)
            {
                groupIngComp6.Visible = true;
            }
        }

        private void btnIngCompanySubmit_Click(object sender, EventArgs e)
        {
            if (txtIngCompanyName.Text != "" || txtIngCompanyAdress.Text != "" || txtIngCompanyPhone.Text != "" || txtIngCompanyEmail.Text != "")
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();

                    string query = "SELECT COUNT(ID) FROM CompanieIngrediente";
                    SqlCommand cmd = new SqlCommand(query, con);
                    string id = Convert.ToString(cmd.ExecuteScalar());

                    query = "EXEC insertCompany '" + id + "', '" + txtIngCompanyName.Text + "', '" + txtIngCompanyAdress.Text + "', " + Convert.ToInt32(txtIngCompanyPhone.Text) + ", '" + txtIngCompanyEmail.Text + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();

                    string idIng;
                    //1st ing
                    if (!groupIngComp1.Visible)
                    {
                        if (txtIngCompName1.Text != "" || txtIngCompQuantity1.Text != "" || txtIngCompDate1.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngCompName1.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertCompanyIngr '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngCompQuantity1.Text) + ",'" + txtIngCompDate1.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //2nd ing
                    if (!groupIngComp2.Visible)
                    {
                        if (txtIngCompName2.Text != "" || txtIngCompQuantity2.Text != "" || txtIngCompDate2.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngCompName2.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertCompanyIngr '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngCompQuantity2.Text) + ",'" + txtIngCompDate2.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //3rd ing
                    if (!groupIngComp3.Visible)
                    {
                        if (txtIngCompName3.Text != "" || txtIngCompQuantity3.Text != "" || txtIngCompDate3.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngCompName3.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertCompanyIngr '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngCompQuantity3.Text) + ",'" + txtIngCompDate3.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //4th ing
                    if (!groupIngComp4.Visible)
                    {
                        if (txtIngCompName4.Text != "" || txtIngCompQuantity4.Text != "" || txtIngCompDate4.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngCompName4.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertCompanyIngr '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngCompQuantity4.Text) + ",'" + txtIngCompDate4.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //5th ing
                    if (!groupIngComp5.Visible)
                    {
                        if (txtIngCompName5.Text != "" || txtIngCompQuantity5.Text != "" || txtIngCompDate5.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngCompName5.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertCompanyIngr '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngCompQuantity5.Text) + ",'" + txtIngCompDate5.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //6th ing
                    if (!groupIngComp6.Visible)
                    {
                        if (txtIngCompName6.Text != "" || txtIngCompQuantity6.Text != "" || txtIngCompDate6.Text != "")
                        {
                            query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngCompName6.Text + "'";
                            cmd = new SqlCommand(query, con);
                            idIng = Convert.ToString(cmd.ExecuteScalar());

                            query = "EXECUTE insertCompanyIngr '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngCompQuantity6.Text) + ",'" + txtIngCompDate6.Text + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    con.Close();
                }
                txtIngCompanyName.Text = string.Empty;
                txtIngCompanyAdress.Text = string.Empty;
                txtIngCompanyEmail.Text = string.Empty;
                txtIngCompanyPhone.Text = string.Empty;
                txtIngCompName1.Text = string.Empty;
                txtIngCompQuantity1.Text = string.Empty;
                txtIngCompDate1.Text = string.Empty;
                txtIngCompName2.Text = string.Empty;
                txtIngCompQuantity2.Text = string.Empty;
                txtIngCompDate2.Text = string.Empty;
                txtIngCompName3.Text = string.Empty;
                txtIngCompQuantity3.Text = string.Empty;
                txtIngCompDate3.Text = string.Empty;
                txtIngCompName4.Text = string.Empty;
                txtIngCompQuantity4.Text = string.Empty;
                txtIngCompDate4.Text = string.Empty;
                txtIngCompName5.Text = string.Empty;
                txtIngCompQuantity5.Text = string.Empty;
                txtIngCompDate5.Text = string.Empty;
                txtIngCompName6.Text = string.Empty;
                txtIngCompQuantity6.Text = string.Empty;
                txtIngCompDate6.Text = string.Empty;
            }
        }

        private void angajatToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlEmployeeUpdate.Size = new Size(411, 323);
            pnlEmployeeUpdate.Visible = true;
            pnlEmployeeUpdate.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlEmployeeUpdate.Width / 2, 24);
            pnlEmployeeUpdate.BringToFront();
            createEmployeeBox();
        }

        private void produsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlProductUpdate.Size = new Size(653, 139);
            pnlProductUpdate.Visible = true;
            pnlProductUpdate.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlProductUpdate.Width / 2, 24);
            pnlProductUpdate.BringToFront();
            createProductComboBox();
            createNumBox();
        }

        private void ingredienteToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlIngredientUpdate.Size = new Size(292, 149);
            pnlIngredientUpdate.Visible = true;
            pnlIngredientUpdate.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlIngredientUpdate.Width / 2, 24);
            pnlIngredientUpdate.BringToFront();
            createIngComboBox();
        }

        private void filialaToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlFacilityUpdate.Size = new Size(254, 179);
            pnlFacilityUpdate.Visible = true;
            pnlFacilityUpdate.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlFacilityUpdate.Width / 2, 24);
            pnlFacilityUpdate.BringToFront();
            createFacilityBox();
        }

        private void masiniDeTransportToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlTransportUpdate.Size = new Size(245, 159);
            pnlTransportUpdate.Visible = true;
            pnlTransportUpdate.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlTransportUpdate.Width / 2, 24);
            pnlTransportUpdate.BringToFront();
            createTransportBox();
        }

        private void companieDeIngredienteToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlIngCompanyUpdate.Size = new Size(668, 481);
            pnlIngCompanyUpdate.Visible = true;
            pnlIngCompanyUpdate.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlIngCompanyUpdate.Width / 2, 24);
            pnlIngCompanyUpdate.BringToFront();
            createBoxNum2();
            createIngCompanyBox();
        }

        private void createEmployeeBox()
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT ID FROM Angajat";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (pnlEmployeeUpdate.Visible)
                    {
                        boxEmployeeUpdate.Items.Add(reader[0]);
                    }
                    if (pnlEmployeeDelete.Visible)
                    {
                        boxEmployeeDelete.Items.Add(reader[0]);
                    }
                }
                con.Close();
            }
        }

        private void btnEmployeeUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query;
                SqlCommand cmd;
                string id = boxEmployeeUpdate.Text;

                if (txtEmployeeNameUpdate.Text != "")
                {
                    query = "UPDATE Angajat SET Nume = '" + txtEmployeeNameUpdate.Text + "' WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (txtEmployeeSurnameUpdate.Text != "")
                {
                    query = "UPDATE Angajat SET Prenume = '" + txtEmployeeSurnameUpdate.Text + "' WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (rBtnMascUpdate.Checked)
                {
                    query = "UPDATE Angajat SET Gen = 'Masc' WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                else if (rBtnFemUpdate.Checked)
                {
                    query = "UPDATE Angajat SET Gen = 'Fem' WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                else if(rBtnOtherUpdate.Checked && txtOtherUpdate.Text != "")
                {
                    query = "UPDATE Angajat SET Gen = '" + txtOtherUpdate.Text + "' WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (txtEmployeePhoneUpdate.Text != "")
                {
                    query = "UPDATE Angajat SET Telefon = '" + txtEmployeePhoneUpdate.Text + "' WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (txtEmployeeAdressUpdate.Text != "")
                {
                    query = "UPDATE Angajat SET Adresa = '" + txtEmployeeAdressUpdate.Text + "' WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (txtEmployeeEmailUpdate.Text != "")
                {
                    query = "UPDATE Angajat SET Email = '" + txtEmployeeEmailUpdate.Text + "' WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (txtEmployeeSalaryUpdate.Text != "")
                {
                    query = "UPDATE Angajat SET Salariu = " + Convert.ToDouble(txtEmployeeSalaryUpdate.Text) + " WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (boxWorkUpdate.Text != "")
                {
                    query = "SELECT ID FROM Functie WHERE Denumire = '" + boxWorkUpdate.Text + "'";
                    cmd = new SqlCommand(query, con);
                    string idWork = Convert.ToString(cmd.ExecuteScalar());

                    query = "UPDATE Angajat SET ID_Functie = '" + idWork + "' WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (boxFacultyUpdate.Text != "")
                {
                    query = "SELECT ID FROM Filiala WHERE Denumire = '" + boxFacultyUpdate.Text + "'";
                    cmd = new SqlCommand(query, con);
                    string idFacility = Convert.ToString(cmd.ExecuteScalar());

                    query = "UPDATE Angajat SET ID_Filiala = '" + idFacility + "' WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            boxEmployeeUpdate.Text = string.Empty;
            txtEmployeeNameUpdate.Text = string.Empty;
            txtEmployeeSurnameUpdate.Text = string.Empty;
            txtEmployeePhoneUpdate.Text = string.Empty;
            txtEmployeeAdressUpdate.Text = string.Empty;
            txtEmployeeEmailUpdate.Text = string.Empty;
            txtEmployeeSalaryUpdate.Text = string.Empty;
            boxWorkUpdate.Text = string.Empty;
            boxFacultyUpdate.Text = string.Empty;
            txtOtherUpdate.Text = string.Empty;
            rBtnFemUpdate.Checked = false;
            rBtnMascUpdate.Checked = false;
            rBtnOtherUpdate.Checked = false;
        }

        private void boxNum3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (boxNum3.SelectedIndex >= 0)
            {
                groupIngUpdate1.Visible = true;
                pnlProductUpdate.Size = new Size(btnProductSubmit.Width + btnProductSubmit.Location.X, groupIngUpdate1.Location.Y + groupIngUpdate1.Height + 50);
                groupIngUpdate2.Visible = false;
                groupIngUpdate3.Visible = false;
                groupIngUpdate4.Visible = false;
                groupIngUpdate5.Visible = false;
                groupIngUpdate6.Visible = false;
                groupIngUpdate7.Visible = false;
                groupIngUpdate8.Visible = false;
                groupIngUpdate9.Visible = false;
                groupIngUpdate10.Visible = false;
                groupIngUpdate11.Visible = false;
                groupIngUpdate12.Visible = false;
                groupIngUpdate13.Visible = false;
            }
            if (boxNum3.SelectedIndex >= 1)
            {
                groupIngUpdate2.Visible = true;
                pnlProductUpdate.Width += groupIngUpdate2.Width + 10;
            }
            if (boxNum3.SelectedIndex >= 2)
            {
                groupIngUpdate3.Visible = true;
                pnlProductUpdate.Height = groupIngUpdate3.Location.Y + groupIngUpdate3.Height + 50;
            }
            if (boxNum3.SelectedIndex >= 3)
            {
                groupIngUpdate4.Visible = true;
            }
            if (boxNum3.SelectedIndex >= 4)
            {
                groupIngUpdate5.Visible = true;
                pnlProductUpdate.Height = groupIngUpdate5.Location.Y + groupIngUpdate5.Height + 50;
            }
            if (boxNum3.SelectedIndex >= 5)
            {
                groupIngUpdate6.Visible = true;
            }
            if (boxNum3.SelectedIndex >= 6)
            {
                groupIngUpdate7.Visible = true;
                pnlProductUpdate.Height = groupIngUpdate7.Location.Y + groupIngUpdate7.Height + 50;
            }
            if (boxNum3.SelectedIndex >= 7)
            {
                groupIngUpdate8.Visible = true;
            }
            if (boxNum3.SelectedIndex >= 8)
            {
                groupIngUpdate9.Visible = true;
                pnlProductUpdate.Height = groupIngUpdate9.Location.Y + groupIngUpdate9.Height + 50;
            }
            if (boxNum3.SelectedIndex >= 9)
            {
                groupIngUpdate10.Visible = true;
            }
            if (boxNum3.SelectedIndex >= 10)
            {
                groupIngUpdate11.Visible = true;
                pnlProductUpdate.Height = groupIngUpdate11.Location.Y + groupIngUpdate11.Height + 50;
            }
            if (boxNum3.SelectedIndex >= 11)
            {
                groupIngUpdate12.Visible = true;
            }
            if (boxNum3.SelectedIndex >= 12)
            {
                groupIngUpdate13.Visible = true;
                pnlProductUpdate.Height = groupIngUpdate13.Location.Y + groupIngUpdate13.Height + 50;
            }
            if (boxNum3.SelectedIndex == 13)
            {
                groupIngUpdate14.Visible = true;
            }
        }

        private void btnProductUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();

                string query = "SELECT ID FROM Produs WHERE Denumire = '" + boxProductUpdate.Text +"'";
                SqlCommand cmd = new SqlCommand(query, con);
                string id = Convert.ToString(cmd.ExecuteScalar());

                if(txtProductCostUpdate.Text != "")
                {
                    query = "UPDATE Produs SET Pret = " + txtProductCostUpdate.Text + " WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }

                if (txtProductDateUpdate.Text != "")
                {
                    query = "UPDATE Produs SET TimpValabilitate = '" + txtProductDateUpdate.Text + "' WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }

                string idIng;
                //1st ing
                if (!groupIngUpdate1.Visible)
                {
                    if (txtIngProdNameUpdate1.Text != "" || txtIngProdQuantityUpdate1.Text != "" || txtIngProdMeasureUpdate1.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdNameUpdate1.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantityUpdate1.Text) + ", '" + txtIngProdMeasureUpdate1.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //2nd ing
                if (!groupIngUpdate2.Visible)
                {
                    if (txtIngProdNameUpdate2.Text != "" || txtIngProdQuantityUpdate2.Text != "" || txtIngProdMeasureUpdate2.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdNameUpdate2.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantityUpdate2.Text) + ",'" + txtIngProdMeasureUpdate2.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //3rd ing
                if (!groupIngUpdate3.Visible)
                {
                    if (txtIngProdNameUpdate3.Text != "" || txtIngProdQuantityUpdate3.Text != "" || txtIngProdMeasureUpdate3.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdNameUpdate3.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantityUpdate3.Text) + ",'" + txtIngProdMeasureUpdate3.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //4th ing
                if (!groupIngUpdate4.Visible)
                {
                    if (txtIngProdNameUpdate4.Text != "" || txtIngProdQuantityUpdate4.Text != "" || txtIngProdMeasureUpdate4.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdNameUpdate4.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantityUpdate4.Text) + ",'" + txtIngProdMeasureUpdate4.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //5th ing
                if (!groupIngUpdate5.Visible)
                {
                    if (txtIngProdNameUpdate5.Text != "" || txtIngProdQuantityUpdate5.Text != "" || txtIngProdMeasureUpdate5.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdNameUpdate5.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantityUpdate5.Text) + ",'" + txtIngProdMeasureUpdate5.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //6th ing
                if (!groupIngUpdate6.Visible)
                {
                    if (txtIngProdNameUpdate6.Text != "" || txtIngProdQuantityUpdate6.Text != "" || txtIngProdMeasureUpdate6.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdNameUpdate6.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantityUpdate6.Text) + ",'" + txtIngProdMeasureUpdate6.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //7th ing
                if (!groupIngUpdate7.Visible)
                {
                    if (txtIngProdNameUpdate7.Text != "" || txtIngProdQuantityUpdate7.Text != "" || txtIngProdMeasureUpdate7.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdNameUpdate7.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantityUpdate7.Text) + ",'" + txtIngProdMeasureUpdate7.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //8th ing
                if (!groupIngUpdate8.Visible)
                {
                    if (txtIngProdNameUpdate8.Text != "" || txtIngProdQuantityUpdate8.Text != "" || txtIngProdMeasureUpdate8.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdNameUpdate8.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantityUpdate8.Text) + ",'" + txtIngProdMeasureUpdate8.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //9th ing
                if (!groupIngUpdate9.Visible)
                {
                    if (txtIngProdNameUpdate9.Text != "" || txtIngProdQuantityUpdate9.Text != "" || txtIngProdMeasureUpdate9.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdNameUpdate9.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantityUpdate9.Text) + ",'" + txtIngProdMeasureUpdate9.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //10th ing
                if (!groupIngUpdate10.Visible)
                {
                    if (txtIngProdNameUpdate10.Text != "" || txtIngProdQuantityUpdate10.Text != "" || txtIngProdMeasureUpdate10.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdNameUpdate10.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantityUpdate10.Text) + ",'" + txtIngProdMeasureUpdate10.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //11th ing
                if (!groupIngUpdate11.Visible)
                {
                    if (txtIngProdNameUpdate11.Text != "" || txtIngProdQuantityUpdate11.Text != "" || txtIngProdMeasureUpdate11.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdNameUpdate11.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantityUpdate11.Text) + ",'" + txtIngProdMeasureUpdate11.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //12th ing
                if (!groupIngUpdate12.Visible)
                {
                    if (txtIngProdNameUpdate12.Text != "" || txtIngProdQuantityUpdate12.Text != "" || txtIngProdMeasureUpdate12.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdNameUpdate12.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantityUpdate12.Text) + ",'" + txtIngProdMeasureUpdate12.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //13th ing
                if (!groupIngUpdate13.Visible)
                {
                    if (txtIngProdNameUpdate13.Text != "" || txtIngProdQuantityUpdate13.Text != "" || txtIngProdMeasureUpdate13.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdNameUpdate13.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantityUpdate13.Text) + ",'" + txtIngProdMeasureUpdate13.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //14th ing
                if (!groupIngUpdate14.Visible)
                {
                    if (txtIngProdNameUpdate14.Text != "" || txtIngProdQuantityUpdate14.Text != "" || txtIngProdMeasureUpdate14.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngProdNameUpdate14.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngProd '" + id + "', '" + idIng + "', " + Convert.ToInt32(txtIngProdQuantityUpdate14.Text) + ",'" + txtIngProdMeasureUpdate14.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            boxProductUpdate.Text = string.Empty;
            txtProductCostUpdate.Text = string.Empty;
            txtProductDateUpdate.Text = string.Empty;
            boxNum3.Text = string.Empty;
            txtIngProdNameUpdate1.Text = string.Empty;
            txtIngProdQuantityUpdate1.Text = string.Empty;
            txtIngProdMeasureUpdate1.Text = string.Empty;
            txtIngProdNameUpdate2.Text = string.Empty;
            txtIngProdQuantityUpdate2.Text = string.Empty;
            txtIngProdMeasureUpdate2.Text = string.Empty;
            txtIngProdNameUpdate3.Text = string.Empty;
            txtIngProdQuantityUpdate3.Text = string.Empty;
            txtIngProdMeasureUpdate3.Text = string.Empty;
            txtIngProdNameUpdate4.Text = string.Empty;
            txtIngProdQuantityUpdate4.Text = string.Empty;
            txtIngProdMeasureUpdate4.Text = string.Empty;
            txtIngProdNameUpdate5.Text = string.Empty;
            txtIngProdQuantityUpdate5.Text = string.Empty;
            txtIngProdMeasureUpdate5.Text = string.Empty;
            txtIngProdNameUpdate6.Text = string.Empty;
            txtIngProdQuantityUpdate6.Text = string.Empty;
            txtIngProdMeasureUpdate6.Text = string.Empty;
            txtIngProdNameUpdate7.Text = string.Empty;
            txtIngProdQuantityUpdate7.Text = string.Empty;
            txtIngProdMeasureUpdate7.Text = string.Empty;
            txtIngProdNameUpdate8.Text = string.Empty;
            txtIngProdQuantityUpdate8.Text = string.Empty;
            txtIngProdMeasureUpdate8.Text = string.Empty;
            txtIngProdNameUpdate9.Text = string.Empty;
            txtIngProdQuantityUpdate9.Text = string.Empty;
            txtIngProdMeasureUpdate9.Text = string.Empty;
            txtIngProdNameUpdate10.Text = string.Empty;
            txtIngProdQuantityUpdate10.Text = string.Empty;
            txtIngProdMeasureUpdate10.Text = string.Empty;
            txtIngProdNameUpdate11.Text = string.Empty;
            txtIngProdQuantityUpdate11.Text = string.Empty;
            txtIngProdMeasureUpdate11.Text = string.Empty;
            txtIngProdNameUpdate12.Text = string.Empty;
            txtIngProdQuantityUpdate12.Text = string.Empty;
            txtIngProdMeasureUpdate12.Text = string.Empty;
            txtIngProdNameUpdate13.Text = string.Empty;
            txtIngProdQuantityUpdate13.Text = string.Empty;
            txtIngProdMeasureUpdate13.Text = string.Empty;
            txtIngProdNameUpdate14.Text = string.Empty;
            txtIngProdQuantityUpdate14.Text = string.Empty;
            txtIngProdMeasureUpdate14.Text = string.Empty;
        }

        private void btnIngredientUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();

                string query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + boxIngredientUpdate.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                string id = Convert.ToString(cmd.ExecuteScalar());

                if (txtIngCostUpdate.Text != "")
                {
                    query = "UPDATE Ingrediente SET Pret = " + txtIngCostUpdate.Text + " WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (txtIngQuantityUpdate.Text != "" && txtIngMeasureUpdate.Text != "")
                {
                    query = "UPDATE Ingrediente SET Cantitate = " + Convert.ToDouble(txtIngQuantityUpdate.Text) + ", Unitate = '" + txtIngMeasureUpdate.Text + "' WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (txtIngDateUpdate.Text != "")
                {
                    query = "UPDATE Ingrediente SET TimpValabilitate = " + Convert.ToInt32(txtIngDateUpdate.Text) + " WHERE ID = '" + id + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            boxIngredientUpdate.Text = string.Empty;
            txtIngCostUpdate.Text = string.Empty;
            txtIngMeasureUpdate.Text = string.Empty;
            txtIngQuantityUpdate.Text = string.Empty;
            txtIngDateUpdate.Text = string.Empty;
        }

        private void createFacilityBox()
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT ID FROM Filiala";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (pnlFacilityUpdate.Visible)
                    {
                        boxFacilityUpdate.Items.Add(reader[0]);
                    }
                    if (pnlFacilityDelete.Visible)
                    {
                        boxFacilityDelete.Items.Add(reader[0]);
                    }
                }
                con.Close();
            }
        }

        private void btnFacilityUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();

                string query;
                SqlCommand cmd;

                if (txtFacilityNameUpdate.Text != "")
                {
                    query = "UPDATE Filiala SET Denumire = '" + txtFacilityNameUpdate.Text + "' WHERE ID = '" + boxFacilityUpdate.Text + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (txtFacilityAdressUpdate.Text != "")
                {
                    query = "UPDATE Filiala SET Adresa = '" + txtFacilityAdressUpdate.Text + "' WHERE ID = '" + boxFacilityUpdate.Text + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (txtFacilityPhoneUpdate.Text != "")
                {
                    query = "UPDATE Filiala SET Telefon = " + txtFacilityPhoneUpdate.Text + " WHERE ID = '" + boxFacilityUpdate.Text + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (txtFacilityEmailUpdate.Text != "")
                {
                    query = "UPDATE Filiala SET Email = " + txtFacilityEmailUpdate.Text + " WHERE ID = '" + boxFacilityUpdate.Text + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            boxFacilityUpdate.Text = string.Empty;
            txtFacilityNameUpdate.Text = string.Empty;
            txtFacilityAdressUpdate.Text = string.Empty;
            txtFacilityPhoneUpdate.Text = string.Empty;
            txtFacilityEmailUpdate.Text = string.Empty;
        }

        private void btnTransportUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();

                string query;
                SqlCommand cmd;

                if (txtTransportTypeUpdate.Text != "")
                {
                    query = "UPDATE MasiniTransport SET Tip = '" + txtTransportTypeUpdate.Text + "' WHERE ID = '" + boxTransportUpdate.Text + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (txtTransportMaxUpdate.Text != "")
                {
                    query = "UPDATE MasiniTransport SET CantitateMax = '" + txtTransportMaxUpdate.Text + "' WHERE ID = '" + boxTransportUpdate.Text + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (txtTransportDriverUpdate.Text != "")
                {
                    string[] name = txtTransportDriverUpdate.Text.Split(' ');
                    query = "SELECT ID FROM Angajat WHERE Nume = '" + name[0] +"' AND Prenume = '" + name[1] +"'";
                    cmd= new SqlCommand(query, con);
                    string driverID = Convert.ToString(cmd.ExecuteScalar());

                    query = "UPDATE MasiniTransport SET ID_Sofer = " + driverID + " WHERE ID = '" + boxTransportUpdate.Text + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }

        private void boxNum4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (boxNum4.SelectedIndex >= 0)
            {
                groupIngCompUpdate1.Visible = true;
                this.Size = new Size(groupIngCompUpdate1.Width + 60, groupIngCompUpdate1.Location.Y + groupIngCompUpdate1.Height + 50);
                pnlIngCompanyUpdate.Width = Screen.PrimaryScreen.WorkingArea.Width;
                groupIngCompUpdate2.Visible = false;
                groupIngCompUpdate3.Visible = false;
                groupIngCompUpdate4.Visible = false;
                groupIngCompUpdate5.Visible = false;
                groupIngCompUpdate6.Visible = false;
            }
            if (boxNum4.SelectedIndex >= 1)
            {
                groupIngCompUpdate2.Visible = true;
                this.Width += groupIngCompUpdate2.Width + 10;
                pnlIngCompanyUpdate.Width = Screen.PrimaryScreen.WorkingArea.Width;
            }
            if (boxNum4.SelectedIndex >= 2)
            {
                groupIngCompUpdate3.Visible = true;
                this.Height = groupIngCompUpdate3.Location.Y + groupIngCompUpdate3.Height + 50;
            }
            if (boxNum4.SelectedIndex >= 3)
            {
                groupIngCompUpdate4.Visible = true;
            }
            if (boxNum4.SelectedIndex >= 4)
            {
                groupIngCompUpdate5.Visible = true;
                this.Height = groupIngCompUpdate5.Location.Y + groupIngCompUpdate5.Height + 50;
            }
            if (boxNum4.SelectedIndex >= 5)
            {
                groupIngCompUpdate6.Visible = true;
            }
        }

        private void btnIngCompanyUpdate_Click(object sender, EventArgs e)
        {
            using(SqlConnection con = new SqlConnection(connection))
            {
                con.Open();

                string query;
                SqlCommand cmd;

                if (txtIngCompanyNameUpdate.Text != "")
                {
                    query = "UPDATE CompanieIngrediente SET Denumire = '" + txtIngCompanyNameUpdate.Text + "' WHERE ID = '" + boxIngCompanyUpdate.Text + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if(txtIngCompanyAdressUpdate.Text != "")
                {
                    query = "UPDATE CompanieIngrediente SET Adresa = '" + txtIngCompanyAdressUpdate.Text + "' WHERE ID = '" + boxIngCompanyUpdate.Text + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (txtIngCompanyPhoneUpdate.Text != "")
                {
                    query = "UPDATE CompanieIngrediente SET Telefon = '" + txtIngCompanyPhoneUpdate.Text + "' WHERE ID = '" + boxIngCompanyUpdate.Text + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                if (txtIngCompanyEmailUpdate.Text != "")
                {
                    query = "UPDATE CompanieIngrediente SET Email = '" + txtIngCompanyEmailUpdate.Text + "' WHERE ID = '" + boxIngCompanyUpdate.Text + "'";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                string idIng;
                //1st ing
                if (!groupIngCompUpdate1.Visible)
                {
                    if (txtIngCompNameUpdate1.Text != "" || txtIngCompQuantityUpdate1.Text != "" || txtIngCompDateUpdate1.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngCompNameUpdate1.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngCompLink '" + boxIngCompanyUpdate.Text + "', '" + idIng + "', " + Convert.ToInt32(txtIngCompQuantityUpdate1.Text) + ", '" + txtIngCompDateUpdate1.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //2nd ing
                if (!groupIngCompUpdate2.Visible)
                {
                    if (txtIngCompNameUpdate2.Text != "" || txtIngProdQuantityUpdate2.Text != "" || txtIngCompDateUpdate2.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngCompQuantityUpdate2.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngCompLink '" + boxIngCompanyUpdate.Text + "', '" + idIng + "', " + Convert.ToInt32(txtIngCompQuantityUpdate2.Text) + ",'" + txtIngCompDateUpdate2.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //3rd ing
                if (!groupIngCompUpdate3.Visible)
                {
                    if (txtIngCompNameUpdate3.Text != "" || txtIngCompQuantityUpdate3.Text != "" || txtIngCompDateUpdate3.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngCompNameUpdate3.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngCompLink '" + boxIngCompanyUpdate.Text + "', '" + idIng + "', " + Convert.ToInt32(txtIngCompQuantityUpdate3.Text) + ",'" + txtIngCompDateUpdate3.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //4th ing
                if (!txtIngCompNameUpdate4.Visible)
                {
                    if (txtIngProdNameUpdate4.Text != "" || txtIngCompQuantityUpdate4.Text != "" || txtIngCompDateUpdate4.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngCompNameUpdate4.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngCompLink '" + boxIngCompanyUpdate.Text + "', '" + idIng + "', " + Convert.ToInt32(txtIngCompQuantityUpdate4.Text) + ",'" + txtIngCompDateUpdate4.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //5th ing
                if (!groupIngCompUpdate5.Visible)
                {
                    if (txtIngCompNameUpdate5.Text != "" || txtIngCompQuantityUpdate5.Text != "" || txtIngCompDateUpdate5.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngCompNameUpdate5.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngCompLink '" + boxIngCompanyUpdate.Text + "', '" + idIng + "', " + Convert.ToInt32(txtIngCompQuantityUpdate5.Text) + ",'" + txtIngCompDateUpdate5.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                //6th ing
                if (!groupIngCompUpdate6.Visible)
                {
                    if (txtIngCompNameUpdate6.Text != "" || txtIngCompQuantityUpdate6.Text != "" || txtIngCompDateUpdate6.Text != "")
                    {
                        query = "SELECT ID FROM Ingrediente WHERE Denumire = '" + txtIngCompNameUpdate6.Text + "'";
                        cmd = new SqlCommand(query, con);
                        idIng = Convert.ToString(cmd.ExecuteScalar());

                        query = "EXECUTE updateIngCompLink '" + boxIngCompanyUpdate.Text + "', '" + idIng + "', " + Convert.ToInt32(txtIngCompQuantityUpdate6.Text) + ",'" + txtIngCompDateUpdate6.Text + "'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                    }
                }

                con.Close();
            }
        }

        private void angajatToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlEmployeeDelete.Visible = true;
            pnlEmployeeDelete.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlEmployeeDelete.Width / 2, 24);
            pnlEmployeeDelete.Size = new Size(260, 53);
            pnlEmployeeDelete.BringToFront();
            createEmployeeBox();
        }

        private void produsToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlProductDelete.Visible = true;
            pnlProductDelete.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlProductDelete.Width / 2, 24);
            pnlProductDelete.Size = new Size(224, 53);
            pnlProductDelete.BringToFront();
            createProductComboBox();
        }

        private void ingredienteToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlIngredientDelete.Visible = true;
            pnlIngredientDelete.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlIngredientDelete.Width / 2, 24);
            pnlIngredientDelete.Size = new Size(224, 53);
            pnlIngredientDelete.BringToFront();
            createIngComboBox();
        }

        private void filialaToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlFacilityDelete.Visible = true;
            pnlFacilityDelete.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlFacilityDelete.Width / 2, 24);
            pnlFacilityDelete.Size = new Size(224, 53);
            pnlFacilityDelete.BringToFront();
            createFacilityBox();
        }

        private void functieToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlWorkDelete.Visible = true;
            pnlWorkDelete.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlWorkDelete.Width / 2, 24);
            pnlWorkDelete.Size = new Size(224, 53);
            pnlWorkDelete.BringToFront();
            createWorkBox();
        }

        private void masiniDeTransportToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlTransportDelete.Visible = true;
            pnlTransportDelete.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlTransportDelete.Width / 2, 24);
            pnlTransportDelete.Size = new Size(224, 53);
            pnlTransportDelete.BringToFront();
            createTransportBox();
        }

        private void companieDeIngredienteToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            hidePanels();
            pnlIngCompanyDelete.Visible = true;
            pnlIngCompanyDelete.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - pnlIngCompanyDelete.Width / 2, 24);
            pnlIngCompanyDelete.Size = new Size(364, 53);
            pnlIngCompanyDelete.BringToFront();
            createIngCompanyBox();
        }

        private void boxEmployeeDelete_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT IDNP FROM Angajat WHERE ID = '" + boxEmployeeDelete.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                lblEmployeeDelete.Text = "IDNP: " + cmd.ExecuteScalar();

                query = "SELECT Nume + ' ' + Prenume FROM Angajat WHERE ID = '" + boxEmployeeDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                lblEmployeeDelete.Text += "\n\nNume: " + Convert.ToString(cmd.ExecuteScalar());

                query = "SELECT Gen FROM Angajat WHERE ID = '" + boxEmployeeDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                lblEmployeeDelete.Text += "\n\nSex: " + Convert.ToString(cmd.ExecuteScalar());

                query = "SELECT DataNastere FROM Angajat WHERE ID = '" + boxEmployeeDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                lblEmployeeDelete.Text += "\n\nData de Nastere:" + Convert.ToString(cmd.ExecuteScalar());

                query = "SELECT Telefon FROM Angajat WHERE ID = '" + boxEmployeeDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                lblEmployeeDelete.Text += "\n\nNumar de Telefon: " + Convert.ToString(cmd.ExecuteScalar());

                query = "SELECT Adresa FROM Angajat WHERE ID = '" + boxEmployeeDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                lblEmployeeDelete.Text += "\n\nAdresa: " + Convert.ToString(cmd.ExecuteScalar());

                query = "SELECT Email FROM Angajat WHERE ID = '" + boxEmployeeDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                lblEmployeeDelete.Text += "\n\nEmail: " + Convert.ToString(cmd.ExecuteScalar());

                query = "SELECT Salariu FROM Angajat WHERE ID = '" + boxEmployeeDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                lblEmployeeDelete.Text += "\n\nSalariu: " + Convert.ToString(cmd.ExecuteScalar()) + " lei";

                query = "SELECT Denumire FROM Functie WHERE ID = (SELECT ID_Functie FROM Angajat WHERE ID = '" + boxEmployeeDelete.Text + "')";
                cmd = new SqlCommand(query, con);
                lblEmployeeDelete.Text += "\n\nFunctie: " + Convert.ToString(cmd.ExecuteScalar());

                query = "SELECT Denumire FROM Filiala WHERE ID = (SELECT ID_Filiala FROM Angajat WHERE ID = '" + boxEmployeeDelete.Text + "')";
                cmd = new SqlCommand(query, con);
                lblEmployeeDelete.Text += "\n\nFiliala: " + Convert.ToString(cmd.ExecuteScalar());

                query = "SELECT Adresa FROM Filiala WHERE ID = (SELECT ID_Filiala FROM Angajat WHERE ID = '" + boxEmployeeDelete.Text + "')";
                cmd = new SqlCommand(query, con);
                lblEmployeeDelete.Text += "\n\n   Adresa: " + Convert.ToString(cmd.ExecuteScalar());

                query = "SELECT Telefon FROM Filiala WHERE ID = (SELECT ID_Filiala FROM Angajat WHERE ID = '" + boxEmployeeDelete.Text + "')";
                cmd = new SqlCommand(query, con);
                lblEmployeeDelete.Text += "\n\n   Telefon: " + Convert.ToString(cmd.ExecuteScalar());
            }
            pnlEmployeeDelete.Size = new Size(lblEmployeeDelete.Width + 50, 402);
        }

        private void btnEmployeeDelete_Click(object sender, EventArgs e)
        {
            using(SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "DELETE FROM Angajat WHERE ID = '" + boxEmployeeDelete.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                lblEmployeeDelete.Text = "Informatia a fost stearsa";
                con.Close();
            }
            boxEmployeeDelete.Text = string.Empty;
        }

        private void boxProductDelete_SelectedIndexChanged(object sender, EventArgs e)
        {

            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT * FROM getIngrediente ('" + boxProductDelete.Text + "')";
                SqlCommand cmd = new SqlCommand(query, con);
                string result = Convert.ToString(cmd.ExecuteScalar());

                string[] productList = result.Split(',');
                lblProductDelete.Text = "Ingrediente: ";
                for (int i = 0; i < productList.Length; i++)
                {
                    lblProductDelete.Text += productList[i] + ",";
                    if (i >= 2 && i % 2 == 0)
                    {
                        lblProductDelete.Text += "\n";
                    }
                }

                query = "SELECT Pret FROM Produs WHERE Denumire = '" + boxProductDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblProductDelete.Text += "\n\nPret: " + String.Format("{0,4:F2}", Convert.ToDouble(result)) + " lei\n";

                query = "SELECT TimpValabilitate FROM Produs WHERE Denumire = '" + boxProductDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblProductDelete.Text += "\nTimp de Valabilitate: " + result + "\n";

                query = "SELECT DataExpirarii FROM Produs WHERE Denumire = '" + boxProductDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblProductDelete.Text += "\nData Expirarii: " + result;

                con.Close();
            }
            pnlProductDelete.Size = new Size(lblProductDelete.Width + 110, lblProductDelete.Height + 110);
        }

        private void btnProductDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "DELETE FROM Produs WHERE Denumire = '" + boxProductDelete.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                lblProductDelete.Text = "Informatia a fost stearsa";
                con.Close();
            }
            boxProductDelete.Text = string.Empty;
        }

        private void boxIngredientDelete_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT * FROM getProduse ('" + boxIngredientDelete.Text + "')";
                SqlCommand cmd = new SqlCommand(query, con);

                string result = Convert.ToString(cmd.ExecuteScalar());
                string[] productList = result.Split(',');
                lblIngredientDelete.Text = "Produse: ";
                for (int i = 0; i < productList.Length; i++)
                {
                    lblIngredientDelete.Text += productList[i] + ",";
                    if (i >= 2 || i % 2 == 0)
                    {
                        lblIngredientDelete.Text += "\n";
                    }
                }
                if (productList.Length % 2 == 0) lblIngredientDelete.Text += "\n";

                query = "SELECT Pret FROM Ingrediente WHERE Denumire = '" + boxIngredientDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblIngredientDelete.Text += "\n\nPret: " + String.Format("{0,4:F2}", Convert.ToDouble(result)) + " lei\n";

                query = "SELECT TimpValabilitate FROM Ingrediente WHERE Denumire = '" + boxIngredientDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblIngredientDelete.Text += "\nTimp de Valabilitate: " + result + "\n";

                query = "SELECT DataExpirarii FROM Ingrediente WHERE Denumire = '" + boxIngredientDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblIngredientDelete.Text += "\nData Expirarii: " + result;

                con.Close();
            }
            pnlIngredientDelete.Size = new Size(lblIngredientDelete.Width + 100, lblIngredientDelete.Height + 110);
        }

        private void btnIngredientDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "DELETE FROM Ingrediente WHERE Denumire = '" + boxIngredientDelete.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                lblIngredientDelete.Text = "Informatia a fost stearsa";
                con.Close();
            }
            boxIngredientDelete.Text = string.Empty;
        }

        private void boxFacilityDelete_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT Denumire FROM Filiala WHERE ID = '" + boxFacilityDelete.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                string result = Convert.ToString(cmd.ExecuteScalar());
                lblFacilityDelete.Text = "\n\nDenumire: " + result + "\n";

                query = "SELECT Adresa FROM Filiala WHERE ID = '" + boxFacilityDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblFacilityDelete.Text += "\nAdresa: " + result + "\n";

                query = "SELECT Telefon FROM Filiala WHERE ID = '" + boxFacilityDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblFacilityDelete.Text += "\nTelefon: " + result + "\n";

                query = "SELECT Email FROM Filiala WHERE ID = '" + boxFacilityDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblFacilityDelete.Text += "\nEmail: " + result + "\n";

                con.Close();
            }
            pnlFacilityDelete.Size = new Size(lblFacilityDelete.Width + 100, lblFacilityDelete.Height + 110);
        }

        private void btnFacilityDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "DELETE FROM Filiala WHERE ID = '" + boxFacilityDelete.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                lblFacilityDelete.Text = "Informatia a fost stearsa";
                con.Close();
            }
            boxFacilityDelete.Text = string.Empty;
        }

        private void createWorkBox()
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT ID FROM Functie";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    boxWorkDelete.Items.Add(reader[0]);
                }
                reader.Close();
                con.Close();
            }
        }

        private void boxWorkDelete_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT Denumire FROM Functie WHERE ID = '" + boxWorkDelete.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                string result = Convert.ToString(cmd.ExecuteScalar());
                lblWorkDelete.Text = "\n\nDenumire: " + result;

                 query = "SELECT * FROM getAngajati ('" + boxWorkDelete.Text + "')";
                 cmd = new SqlCommand(query, con);
                 result = Convert.ToString(cmd.ExecuteScalar());
                 lblWorkDelete.Text += "\n   " + result + "\n";

                con.Close();
            }
            pnlWorkDelete.Size = new Size(lblWorkDelete.Width + 100, lblWorkDelete.Height + 110);
        }

        private void btnWorkDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "DELETE FROM Functie WHERE ID = '" + boxWorkDelete.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                lblWorkDelete.Text = "Informatia a fost stearsa";
                con.Close();
            }
            boxWorkDelete.Text = string.Empty;
        }

        private void boxTransportDelete_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT Tip FROM MasiniTransport WHERE ID = '" + boxTransportDelete.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                string result = Convert.ToString(cmd.ExecuteScalar());
                lblTransportDelete.Text = "Tip: " + result + "\n";

                query = "SELECT CantitateMax FROM MasiniTransport WHERE ID = '" + boxTransportDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblTransportDelete.Text += "\nCantitate max: " + result + "\n";

                query = "SELECT Nume + ' ' + Prenume AS NumeAngajat FROM Angajat WHERE ID = (SELECT ID_Sofer FROM MasiniTransport WHERE ID = '" + boxTransportDelete.Text + "')";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblTransportDelete.Text += "\nSofer: " + result + "\n";

                con.Close();
            }
            pnlTransportDelete.Size = new Size(224, lblTransportDelete.Height + 120);
        }

        private void btnTransportDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "DELETE FROM MasiniTransport WHERE ID = '" + boxTransportDelete.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                lblTransportDelete.Text = "Informatia a fost stearsa";
                con.Close();
            }
            boxTransportDelete.Text = string.Empty;
        }

        private void boxIngCompanyDelete_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "SELECT Adresa FROM CompanieIngrediente WHERE Denumire = '" + boxIngCompanyDelete.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                string result = Convert.ToString(cmd.ExecuteScalar());
                lblIngCompanyDelete.Text = "Adresa: " + result;

                query = "SELECT Telefon FROM CompanieIngrediente WHERE Denumire = '" + boxIngCompanyDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblIngCompanyDelete.Text += "\n\nTelefon: " + result;

                query = "SELECT Email FROM CompanieIngrediente WHERE Denumire = '" + boxIngCompanyDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblIngCompanyDelete.Text += "\n\nEmail: " + result;

                query = "SELECT ID FROM CompanieIngrediente WHERE Denumire = '" + boxIngCompanyDelete.Text + "'";
                cmd = new SqlCommand(query, con);
                string id = Convert.ToString(cmd.ExecuteScalar());

                query = "SELECT STRING_AGG(Denumire, ', ') AS IngredienteDenumiri FROM Ingrediente WHERE ID IN (SELECT ID_Ingrediente FROM IngredienteLinkComp WHERE ID_CompanieIngrediente = '" + id + "')";
                cmd = new SqlCommand(query, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                lblIngCompanyDelete.Text += "\n\nIngrediente: " + result;

                con.Close();
            }
            pnlIngCompanyDelete.Size = new Size(364, lblIngCompanyDelete.Height + 100);
        }

        private void btnIngCompanyDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "DELETE FROM MasiniTransport WHERE ID = '" + boxIngCompanyDelete.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                lblIngCompanyDelete.Text = "Informatia a fost stearsa";
                con.Close();
            }
            boxIngCompanyDelete.Text = string.Empty;
        }
    }
}
