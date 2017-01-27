using System;
using System.Windows.Forms;
using TNT.Services.Contracts;
using TNT.Services.Objects;

namespace TNT.Services.Registration
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			System.Windows.Forms.Application.Idle += new EventHandler(Application_Idle);
		}

		void Application_Idle(object sender, EventArgs e)
		{
			RegistrationButton.Enabled = ApplicationIDComboBox.SelectedIndex > -1 && !string.IsNullOrEmpty(NameTextBox.Text);
		}

		private void RegistrationButton_Click(object sender, EventArgs e)
		{
			TNTServicesClient client = new TNTServicesClient("BasicHttpBinding_ITNTServicesContracts");
			User user = new Objects.User()
			{
				Address = StreetAddressTextBox.Text,
				City = CityTextBox.Text,
				EmailAddress = EmailAddressTextBox.Text,
				Name = NameTextBox.Text,
				PhoneNumber = PhoneNumberTextBox.Text,
				State = StateTextBox.Text,
				Zip = ZipTextBox.Text
			};

			Objects.License license = client.RequestLicense(user, new Guid(ApplicationIDComboBox.SelectedItem.ToString()));

			Clipboard.SetText(license.Key);

			MessageBox.Show(this, string.Format("{0}\n\nAlso copied to clipboard.", license.Key), "License Key");
		}
	}
}
