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

namespace csod_edge_integrations_custom_provider_service.Models.BackgroundCheck
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class ProfessionalExperience :  IEquatable<ProfessionalExperience>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfessionalExperience" /> class.
        /// </summary>
        /// <param name="Position">Position.</param>
        /// <param name="Organization">Organization.</param>
        /// <param name="Description">Description.</param>
        /// <param name="StartDate">StartDate.</param>
        /// <param name="EndDate">EndDate.</param>
        /// <param name="Current">Current.</param>
        /// <param name="DateRange">DateRange.</param>
        public ProfessionalExperience(string Position = null, string Organization = null, string Description = null, string StartDate = null, string EndDate = null, string Current = null, string DateRange = null)
        {
            this.Position = Position;
            this.Organization = Organization;
            this.Description = Description;
            this.StartDate = StartDate;
            this.EndDate = EndDate;
            this.Current = Current;
            this.DateRange = DateRange;
            
        }

        /// <summary>
        /// Gets or Sets Position
        /// </summary>
        [DataMember(Name="position")]
        public string Position { get; set; }

        /// <summary>
        /// Gets or Sets Organization
        /// </summary>
        [DataMember(Name="organization")]
        public string Organization { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name="description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets StartDate
        /// </summary>
        [DataMember(Name="startDate")]
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or Sets EndDate
        /// </summary>
        [DataMember(Name="endDate")]
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or Sets Current
        /// </summary>
        [DataMember(Name="current")]
        public string Current { get; set; }

        /// <summary>
        /// Gets or Sets DateRange
        /// </summary>
        [DataMember(Name="dateRange")]
        public string DateRange { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ProfessionalExperience {\n");
            sb.Append("  Position: ").Append(Position).Append("\n");
            sb.Append("  Organization: ").Append(Organization).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  EndDate: ").Append(EndDate).Append("\n");
            sb.Append("  Current: ").Append(Current).Append("\n");
            sb.Append("  DateRange: ").Append(DateRange).Append("\n");
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
            return Equals((ProfessionalExperience)obj);
        }

        /// <summary>
        /// Returns true if ProfessionalExperience instances are equal
        /// </summary>
        /// <param name="other">Instance of ProfessionalExperience to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ProfessionalExperience other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    this.Position == other.Position ||
                    this.Position != null &&
                    this.Position.Equals(other.Position)
                ) && 
                (
                    this.Organization == other.Organization ||
                    this.Organization != null &&
                    this.Organization.Equals(other.Organization)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) && 
                (
                    this.StartDate == other.StartDate ||
                    this.StartDate != null &&
                    this.StartDate.Equals(other.StartDate)
                ) && 
                (
                    this.EndDate == other.EndDate ||
                    this.EndDate != null &&
                    this.EndDate.Equals(other.EndDate)
                ) && 
                (
                    this.Current == other.Current ||
                    this.Current != null &&
                    this.Current.Equals(other.Current)
                ) && 
                (
                    this.DateRange == other.DateRange ||
                    this.DateRange != null &&
                    this.DateRange.Equals(other.DateRange)
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
                if (this.Position != null)
                    hash = hash * 59 + this.Position.GetHashCode();
                if (this.Organization != null)
                    hash = hash * 59 + this.Organization.GetHashCode();
                if (this.Description != null)
                    hash = hash * 59 + this.Description.GetHashCode();
                if (this.StartDate != null)
                    hash = hash * 59 + this.StartDate.GetHashCode();
                if (this.EndDate != null)
                    hash = hash * 59 + this.EndDate.GetHashCode();
                if (this.Current != null)
                    hash = hash * 59 + this.Current.GetHashCode();
                if (this.DateRange != null)
                    hash = hash * 59 + this.DateRange.GetHashCode();
                return hash;
            }
        }

        #region Operators

        public static bool operator ==(ProfessionalExperience left, ProfessionalExperience right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProfessionalExperience left, ProfessionalExperience right)
        {
            return !Equals(left, right);
        }

        #endregion Operators

    }
}
