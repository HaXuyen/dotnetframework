using AppG2.Controller;
using AppG2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppG2.View
{
    public partial class frmContact : Form
    {
        #region Path data file
        string pathContactFile;
        #endregion
        public frmContact()
        {
            InitializeComponent();
            pathContactFile = Application.StartupPath + @"\Data\contact.txt";

            bdsContact.DataSource = null;
            dtgContact.AutoGenerateColumns = false;

            var listContactNoSort = ContactService.GetAllContact(pathContactFile);
            var listContact = listContactNoSort.OrderBy(x => x.Name).ToList();
            if (listContact == null)
                throw new Exception("Không có liên lạc nào");
            else
            {
                bdsContact.DataSource = listContact;
            }
            dtgContact.DataSource = bdsContact;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
           
            var f = new frmContactInfo(null); ;
            if (f.ShowDialog() == DialogResult.OK)
            {
                var newContactListNoSort = ContactService.GetAllContact(pathContactFile);
                var newContactList = newContactListNoSort.OrderBy(x => x.Name);
                bdsContact.DataSource = newContactList;
                bdsContact.ResetBindings(true);
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            var contact = bdsContact.Current as Contact;
            if (contact != null)
            {
                var f = new frmContactInfo(contact);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    var newContactListNoSort = ContactService.GetAllContact(pathContactFile);
                    var newContactList = newContactListNoSort.OrderBy(x => x.Name);
                    bdsContact.DataSource = newContactList;
                    bdsContact.ResetBindings(true);
                }
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            var rs = MessageBox.Show("Bạn có chắc muốn xóa liên hệ này không?",
                "Thông báo",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning);
            if (rs == DialogResult.OK)
            {
                var contact = (Contact)bdsContact.Current;
                List<Contact> listContact = (List<Contact>)bdsContact.DataSource;
                listContact.Remove(contact);
                bdsContact.DataSource = listContact;
                bdsContact.ResetBindings(true);
                String delete = contact.Id + "#" + contact.Name + "#" + contact.Phone + "#" + contact.Email;
                var Lines = File.ReadAllLines(pathContactFile);
                var newLines = Lines.Where(line => !line.Contains(delete));
                File.WriteAllLines(pathContactFile, newLines);
                MessageBox.Show("Bạn đã xóa thành công. Name: " + contact.Name);
            }
            else
            {
                MessageBox.Show("Bạn đã hủy bỏ");
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            var contactListNoSort = ContactService.GetContactBySearch(txtSearch.Text, pathContactFile);
            bdsContact.DataSource = contactListNoSort;
            bdsContact.ResetBindings(true);
        }
    }
}
