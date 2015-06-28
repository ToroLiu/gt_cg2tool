using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFCG2Tool
{
    using Strategies;

    public partial class FormOption : Form
    {
        public FormOption()
        {
            InitializeComponent();

            txtTime.KeyPress += txtKeyPressed;
            txtLoopMax.KeyPress += txtKeyPressed;

            // Initialize
            var conf = MySetting.Instance;
            conf.Load();

            txtTime.Text = conf.MaxSeconds.ToString();
            txtLoopMax.Text = conf.LoopMax.ToString();
            chkQuitTeam.Checked = conf.QuitTeam;

            cboStrategy.Items.Clear();

            List<OptionStrategy> options = new List<OptionStrategy> {
                new OptionStrategy(DIR_STATEGY.UP_DOWN, "上下移動"),
                new OptionStrategy(DIR_STATEGY.LEFT_RIGHT, "左右移動"),
                new OptionStrategy(DIR_STATEGY.CIRCLE, "繞圈圈"),
            };

            cboStrategy.Items.AddRange(options.ToArray());
            DIR_STATEGY selected = conf.Strategy;
            OptionStrategy sel = options.SingleOrDefault(o => o.Strategy == selected);
            cboStrategy.SelectedItem = sel;
        }

        private void txtKeyPressed(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) {
                e.Handled = true;     
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Save configuration
            int loopMax = Convert.ToInt32(txtLoopMax.Text);
            int maxSeconds = Convert.ToInt32(txtTime.Text);

            OptionStrategy selStra = (OptionStrategy)cboStrategy.SelectedItem;

            MySetting conf = MySetting.Instance;
            conf.LoopMax = loopMax;
            conf.MaxSeconds = maxSeconds;
            conf.Strategy = selStra.Strategy;

            conf.QuitTeam = chkQuitTeam.Checked;

            conf.Save();

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
