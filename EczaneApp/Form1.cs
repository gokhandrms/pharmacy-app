using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace EczaneApp
{
    public partial class Form1 : Form
    {
        OleDbConnection con = new OleDbConnection("provider = microsoft.ace.oledb.12.0;data source = eczaneDB.accdb");
        OleDbCommand cmd;
        OleDbDataAdapter da;

        void Listele()
        {
            da = new OleDbDataAdapter("select * from ilaclar", con);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            Temizle();
        }

        public void Temizle()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            pictureBox1.ImageLocation = "";
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(pictureBox1.ImageLocation))
            {
                MessageBox.Show("Lütfen tüm alanlarý doldurunuz", "EKSÝK GÝRÝÞ HATASI");
                return;
            }

            try
            {
                cmd = new OleDbCommand("INSERT INTO ilaclar (siraNo, ilacKodu, ilacAdi, fiyat, adet, resim) VALUES (@siraNo, @ilacKodu, @ilacAdi, @fiyat, @adet, @resim)", con);
                cmd.Parameters.AddWithValue("@siraNo", int.Parse(textBox1.Text));
                cmd.Parameters.AddWithValue("@ilacKodu", int.Parse(textBox2.Text));
                cmd.Parameters.AddWithValue("@ilacAdi", textBox3.Text);
                cmd.Parameters.AddWithValue("@fiyat", int.Parse(textBox4.Text));
                cmd.Parameters.AddWithValue("@adet", int.Parse(textBox5.Text));
                cmd.Parameters.AddWithValue("@resim", pictureBox1.ImageLocation);

                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Kayýt yapýldý");
                Listele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "HATA");
            }
            finally
            {
                con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim dosyalarý | *.jpg;*.png";
            if (dosya.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = dosya.FileName;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                textBox4.Text = row.Cells[3].Value.ToString();
                textBox5.Text = row.Cells[4].Value.ToString();
                pictureBox1.ImageLocation = row.Cells[5].Value.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cmd = new OleDbCommand("update ilaclar set ilacKodu = '" + textBox2.Text + "',ilacAdi = '" + textBox3.Text + "',fiyat ='" + textBox4.Text + "',adet ='" + textBox5.Text + "',resim'" + pictureBox1.ImageLocation + "'where siraNo = " + textBox1.Text + "  ", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Güncelleme tamam");
            Listele();


        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Lütfen silmek istediðiniz kaydýn sýra numarasýný giriniz", "EKSÝK GÝRÝÞ HATASI");
                return;
            }

            try
            {
                cmd = new OleDbCommand("DELETE FROM ilaclar WHERE siraNo = @siraNo", con);
                cmd.Parameters.AddWithValue("@siraNo", int.Parse(textBox1.Text));

                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Silme tamam");
                Listele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "HATA");
            }
            finally
            {
                con.Close();
            }
        }
    }
}
