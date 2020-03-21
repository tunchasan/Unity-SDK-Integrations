using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using DownloadTexture = VoxelBusters.Utility.DownloadTexture;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Represents an immutable object of contact properties, such as contact’s name, image, phone numbers etc.  
	/// </summary>
	[System.Serializable]
	public class AddressBookContact
	{
		#region Static Fields

		private static Texture2D		defaultImage;

		#endregion

		#region Fields

		[SerializeField]
		private 		string 			m_firstName;
		[SerializeField]
		private 		string 			m_lastName;
		[SerializeField]
		private			Texture2D		m_image;
		private			string			m_imageDownloadError;
		[SerializeField]
		private 		string[]		m_phoneNumberList;
		[SerializeField]
		private 		string[]		m_emailIDList;

		private			DownloadTexture	m_downloadRequest;

		#endregion

		#region Properties
		
		/// <summary>
		/// The first name of the contact. (read-only)
		///	</summary>
		public string FirstName
		{
			get 
			{ 
				return m_firstName; 
			}

			protected set 
			{ 
				m_firstName = value; 
			}
		}

		/// <summary>
		/// The last name of the contact. (read-only)
		///	</summary>
		public string LastName
		{
			get 
			{ 
				return m_lastName; 
			}

			protected set 
			{ 
				m_lastName = value; 
			}
		}

		protected string ImagePath
		{
			get;
			set;
		}
	
		/// <summary>
		/// An array of phone numbers of the contact. (read-only)
		/// </summary>
		public string[] PhoneNumberList
		{
			get 
			{ 
				return m_phoneNumberList; 
			}

			protected set 
			{ 
				m_phoneNumberList = value; 
			}
		}

		/// <summary>
		/// An array of email addresses of the contact. (read-only)
		/// </summary>
		public string[] EmailIDList
		{
			get 
			{ 
				return m_emailIDList; 
			}

			protected set 
			{ 
				m_emailIDList = value; 
			}
		}

		#endregion

		#region Constructors

		protected AddressBookContact ()
		{
			this.FirstName				= null;
			this.LastName				= null;
			this.ImagePath				= null;
			this.m_image				= null;
			this.m_imageDownloadError	= null;
			this.PhoneNumberList		= new string[0];
			this.EmailIDList			= new string[0];
		}

		protected AddressBookContact (AddressBookContact _source)
		{
			this.FirstName				= _source.FirstName;
			this.LastName				= _source.LastName;
			this.ImagePath				= _source.ImagePath;
			this.m_image				= _source.m_image;
			this.m_imageDownloadError	= _source.m_imageDownloadError;
			this.PhoneNumberList		= _source.PhoneNumberList;
			this.EmailIDList			= _source.EmailIDList;
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Returns the default profile picture for contacts.
		/// </summary>
		/// <returns>The default profile picture for contacts.</returns>
		public static Texture2D GetDefaultImage ()
		{
			if (defaultImage == null)
				defaultImage	= Resources.Load(Constants.kDefaultContactImagePath) as Texture2D;

			return defaultImage;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Asynchronously loads the profile picture of a contact.
		/// </summary>
		/// <param name="_onCompletion">Sent when request to load image is finished.</param>
		public void GetImageAsync (DownloadTexture.Completion _onCompletion)
		{
			// Use cached information
			if (m_image != null || m_imageDownloadError != null)
			{
				_onCompletion(m_image, m_imageDownloadError);
				return;
			}
			else if (string.IsNullOrEmpty(ImagePath))
			{
				_onCompletion(GetDefaultImage(), null);
				return;
			}
			else if (m_downloadRequest == null)
			{
				URL _imagePathURL	= URL.FileURLWithPath(ImagePath);

				// Start request
				m_downloadRequest	= new DownloadTexture(_imagePathURL, true, true);
				m_downloadRequest.OnCompletion	= (Texture2D _newTexture, string _error) => {

					// Update properties
					m_image 				= _newTexture;
					m_imageDownloadError	= _error;
					m_downloadRequest		= null;

					// Send callback
					if (_onCompletion != null)
						_onCompletion(_newTexture, _error);
				};
				m_downloadRequest.StartRequest();
			}
		}

		public override string ToString ()
		{
			return string.Format("[AddressBookContact: FirstName={0}, LastName={1}]", FirstName, LastName);
		}

		#endregion
	}
}