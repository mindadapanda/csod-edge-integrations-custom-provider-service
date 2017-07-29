/*
 * Edge.Integration.Host.BackgroundCheck
 *
 * Edge Custom Background Check API Template. Use this template to help generate your API contracts so that you can connect with CSOD and become a custom Background Check provider. Detailed in this API are endpoints that should be implemented so that the contract can adhere to Edge Custom Background Check Provier standards
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace csod_edge_integrations_custom_provider_service.Models.EdgeBackgroundCheck
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class ApplicantData : IEquatable<ApplicantData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicantData" /> class.
        /// </summary>
        /// <param name="FirstName">FirstName (required).</param>
        /// <param name="LastName">LastName (required).</param>
        /// <param name="MiddleName">MiddleName.</param>
        /// <param name="NamePrefix">prefix.</param>
        /// <param name="NameSuffix">suffix.</param>
        /// <param name="RecruiterEmail">email of the person requesting the background check.</param>
        /// <param name="ContactInfo">ContactInfo.</param>
        /// <param name="Address">Address.</param>
        /// <param name="Resume">Resume.</param>
        public ApplicantData(string FirstName = null, string LastName = null, string MiddleName = null, string NamePrefix = null, string NameSuffix = null, string RecruiterEmail = null, ApplicantDataContactInfo ContactInfo = null, ApplicantDataAddress Address = null, ApplicantDataResume Resume = null)
        {
            // to ensure "FirstName" is required (not null)
            if (FirstName == null)
            {
                throw new InvalidDataException("FirstName is a required property for ApplicantData and cannot be null");
            }
            else
            {
                this.FirstName = FirstName;
            }
            // to ensure "LastName" is required (not null)
            if (LastName == null)
            {
                throw new InvalidDataException("LastName is a required property for ApplicantData and cannot be null");
            }
            else
            {
                this.LastName = LastName;
            }
            this.MiddleName = MiddleName;
            this.NamePrefix = NamePrefix;
            this.NameSuffix = NameSuffix;
            this.RecruiterEmail = RecruiterEmail;
            this.ContactInfo = ContactInfo;
            this.Address = Address;
            this.Resume = Resume;

        }

        /// <summary>
        /// Gets or Sets FirstName
        /// </summary>
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or Sets LastName
        /// </summary>
        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or Sets MiddleName
        /// </summary>
        [DataMember(Name = "middleName")]
        public string MiddleName { get; set; }

        /// <summary>
        /// prefix
        /// </summary>
        /// <value>prefix</value>
        [DataMember(Name = "namePrefix")]
        public string NamePrefix { get; set; }

        /// <summary>
        /// suffix
        /// </summary>
        /// <value>suffix</value>
        [DataMember(Name = "nameSuffix")]
        public string NameSuffix { get; set; }

        /// <summary>
        /// email of the person requesting the background check
        /// </summary>
        /// <value>email of the person requesting the background check</value>
        [DataMember(Name = "recruiterEmail")]
        public string RecruiterEmail { get; set; }

        /// <summary>
        /// Gets or Sets ContactInfo
        /// </summary>
        [DataMember(Name = "contactInfo")]
        public ApplicantDataContactInfo ContactInfo { get; set; }

        /// <summary>
        /// Gets or Sets Address
        /// </summary>
        [DataMember(Name = "address")]
        public ApplicantDataAddress Address { get; set; }

        /// <summary>
        /// Gets or Sets Resume
        /// </summary>
        [DataMember(Name = "resume")]
        public ApplicantDataResume Resume { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApplicantData {\n");
            sb.Append("  FirstName: ").Append(FirstName).Append("\n");
            sb.Append("  LastName: ").Append(LastName).Append("\n");
            sb.Append("  MiddleName: ").Append(MiddleName).Append("\n");
            sb.Append("  NamePrefix: ").Append(NamePrefix).Append("\n");
            sb.Append("  NameSuffix: ").Append(NameSuffix).Append("\n");
            sb.Append("  RecruiterEmail: ").Append(RecruiterEmail).Append("\n");
            sb.Append("  ContactInfo: ").Append(ContactInfo).Append("\n");
            sb.Append("  Address: ").Append(Address).Append("\n");
            sb.Append("  Resume: ").Append(Resume).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ApplicantData)obj);
        }

        /// <summary>
        /// Returns true if ApplicantData instances are equal
        /// </summary>
        /// <param name="other">Instance of ApplicantData to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApplicantData other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    this.FirstName == other.FirstName ||
                    this.FirstName != null &&
                    this.FirstName.Equals(other.FirstName)
                ) &&
                (
                    this.LastName == other.LastName ||
                    this.LastName != null &&
                    this.LastName.Equals(other.LastName)
                ) &&
                (
                    this.MiddleName == other.MiddleName ||
                    this.MiddleName != null &&
                    this.MiddleName.Equals(other.MiddleName)
                ) &&
                (
                    this.NamePrefix == other.NamePrefix ||
                    this.NamePrefix != null &&
                    this.NamePrefix.Equals(other.NamePrefix)
                ) &&
                (
                    this.NameSuffix == other.NameSuffix ||
                    this.NameSuffix != null &&
                    this.NameSuffix.Equals(other.NameSuffix)
                ) &&
                (
                    this.RecruiterEmail == other.RecruiterEmail ||
                    this.RecruiterEmail != null &&
                    this.RecruiterEmail.Equals(other.RecruiterEmail)
                ) &&
                (
                    this.ContactInfo == other.ContactInfo ||
                    this.ContactInfo != null &&
                    this.ContactInfo.Equals(other.ContactInfo)
                ) &&
                (
                    this.Address == other.Address ||
                    this.Address != null &&
                    this.Address.Equals(other.Address)
                ) &&
                (
                    this.Resume == other.Resume ||
                    this.Resume != null &&
                    this.Resume.Equals(other.Resume)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)
                if (this.FirstName != null)
                    hash = hash * 59 + this.FirstName.GetHashCode();
                if (this.LastName != null)
                    hash = hash * 59 + this.LastName.GetHashCode();
                if (this.MiddleName != null)
                    hash = hash * 59 + this.MiddleName.GetHashCode();
                if (this.NamePrefix != null)
                    hash = hash * 59 + this.NamePrefix.GetHashCode();
                if (this.NameSuffix != null)
                    hash = hash * 59 + this.NameSuffix.GetHashCode();
                if (this.RecruiterEmail != null)
                    hash = hash * 59 + this.RecruiterEmail.GetHashCode();
                if (this.ContactInfo != null)
                    hash = hash * 59 + this.ContactInfo.GetHashCode();
                if (this.Address != null)
                    hash = hash * 59 + this.Address.GetHashCode();
                if (this.Resume != null)
                    hash = hash * 59 + this.Resume.GetHashCode();
                return hash;
            }
        }

        #region Operators

        public static bool operator ==(ApplicantData left, ApplicantData right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ApplicantData left, ApplicantData right)
        {
            return !Equals(left, right);
        }

        #endregion Operators

    }
}
