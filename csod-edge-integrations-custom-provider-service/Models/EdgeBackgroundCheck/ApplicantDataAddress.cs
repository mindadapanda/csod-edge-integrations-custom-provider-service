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
    public partial class ApplicantDataAddress :  IEquatable<ApplicantDataAddress>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicantDataAddress" /> class.
        /// </summary>
        /// <param name="AddressLine1">address.</param>
        /// <param name="AddressLine2">address continued.</param>
        /// <param name="AddressCity">city.</param>
        /// <param name="AddressState">state or province.</param>
        /// <param name="AddressPostalZipCode">zip code.</param>
        /// <param name="AddressCountryCode">country code in 2 letters.</param>
        public ApplicantDataAddress(string AddressLine1 = null, string AddressLine2 = null, string AddressCity = null, string AddressState = null, string AddressPostalZipCode = null, string AddressCountryCode = null)
        {
            this.AddressLine1 = AddressLine1;
            this.AddressLine2 = AddressLine2;
            this.AddressCity = AddressCity;
            this.AddressState = AddressState;
            this.AddressPostalZipCode = AddressPostalZipCode;
            this.AddressCountryCode = AddressCountryCode;
            
        }

        /// <summary>
        /// address
        /// </summary>
        /// <value>address</value>
        [DataMember(Name="addressLine1")]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// address continued
        /// </summary>
        /// <value>address continued</value>
        [DataMember(Name="addressLine2")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// city
        /// </summary>
        /// <value>city</value>
        [DataMember(Name="addressCity")]
        public string AddressCity { get; set; }

        /// <summary>
        /// state or province
        /// </summary>
        /// <value>state or province</value>
        [DataMember(Name="addressState")]
        public string AddressState { get; set; }

        /// <summary>
        /// zip code
        /// </summary>
        /// <value>zip code</value>
        [DataMember(Name="addressPostalZipCode")]
        public string AddressPostalZipCode { get; set; }

        /// <summary>
        /// country code in 2 letters
        /// </summary>
        /// <value>country code in 2 letters</value>
        [DataMember(Name="addressCountryCode")]
        public string AddressCountryCode { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApplicantDataAddress {\n");
            sb.Append("  AddressLine1: ").Append(AddressLine1).Append("\n");
            sb.Append("  AddressLine2: ").Append(AddressLine2).Append("\n");
            sb.Append("  AddressCity: ").Append(AddressCity).Append("\n");
            sb.Append("  AddressState: ").Append(AddressState).Append("\n");
            sb.Append("  AddressPostalZipCode: ").Append(AddressPostalZipCode).Append("\n");
            sb.Append("  AddressCountryCode: ").Append(AddressCountryCode).Append("\n");
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
            return Equals((ApplicantDataAddress)obj);
        }

        /// <summary>
        /// Returns true if ApplicantDataAddress instances are equal
        /// </summary>
        /// <param name="other">Instance of ApplicantDataAddress to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApplicantDataAddress other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    this.AddressLine1 == other.AddressLine1 ||
                    this.AddressLine1 != null &&
                    this.AddressLine1.Equals(other.AddressLine1)
                ) && 
                (
                    this.AddressLine2 == other.AddressLine2 ||
                    this.AddressLine2 != null &&
                    this.AddressLine2.Equals(other.AddressLine2)
                ) && 
                (
                    this.AddressCity == other.AddressCity ||
                    this.AddressCity != null &&
                    this.AddressCity.Equals(other.AddressCity)
                ) && 
                (
                    this.AddressState == other.AddressState ||
                    this.AddressState != null &&
                    this.AddressState.Equals(other.AddressState)
                ) && 
                (
                    this.AddressPostalZipCode == other.AddressPostalZipCode ||
                    this.AddressPostalZipCode != null &&
                    this.AddressPostalZipCode.Equals(other.AddressPostalZipCode)
                ) && 
                (
                    this.AddressCountryCode == other.AddressCountryCode ||
                    this.AddressCountryCode != null &&
                    this.AddressCountryCode.Equals(other.AddressCountryCode)
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
                if (this.AddressLine1 != null)
                    hash = hash * 59 + this.AddressLine1.GetHashCode();
                if (this.AddressLine2 != null)
                    hash = hash * 59 + this.AddressLine2.GetHashCode();
                if (this.AddressCity != null)
                    hash = hash * 59 + this.AddressCity.GetHashCode();
                if (this.AddressState != null)
                    hash = hash * 59 + this.AddressState.GetHashCode();
                if (this.AddressPostalZipCode != null)
                    hash = hash * 59 + this.AddressPostalZipCode.GetHashCode();
                if (this.AddressCountryCode != null)
                    hash = hash * 59 + this.AddressCountryCode.GetHashCode();
                return hash;
            }
        }

        #region Operators

        public static bool operator ==(ApplicantDataAddress left, ApplicantDataAddress right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ApplicantDataAddress left, ApplicantDataAddress right)
        {
            return !Equals(left, right);
        }

        #endregion Operators

    }
}
