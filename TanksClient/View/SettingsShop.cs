using System;
using System.Windows.Forms;

namespace TanksClient.View
{
    public partial class SettingsShop : Form
    {
        public SettingsShop()
        {
            InitializeComponent();
            this.AutoSize = true;
            this.label7.Text = MainMenuForm.Money.ToString();
            this.label1.Text = MainMenuForm.Health == 10 ? "Max" : MainMenuForm.Health.ToString();
            this.label2.Text = MainMenuForm.Damage == 10 ? "Max" : MainMenuForm.Damage.ToString();
            this.label3.Text = MainMenuForm.Speed == 10 ? "Max" : MainMenuForm.Speed.ToString();
            this.checkBox1.CheckedChanged += updateDebug;
        }

        private void updateDebug(object sender, EventArgs e)
        {
            MainMenuForm.Debug = this.checkBox1.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //speed
            if (MainMenuForm.Money >= 10 && MainMenuForm.Speed != 0)
            {
                MainMenuForm.Money -= 10;
                MainMenuForm.Speed += 1;
                this.Invalidate();
                this.Update();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //dmg
            if (MainMenuForm.Money >= 10 && MainMenuForm.Damage != 0)
            {
                MainMenuForm.Money -= 10;
                MainMenuForm.Damage += 1;
                this.Invalidate();
                this.Update();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //hp
            if (MainMenuForm.Money >= 10 && MainMenuForm.Health != 0)
            {
                MainMenuForm.Money -= 10;
                MainMenuForm.Health += 1;
                this.Invalidate();
                this.Update();
            }
        }
    }
}
