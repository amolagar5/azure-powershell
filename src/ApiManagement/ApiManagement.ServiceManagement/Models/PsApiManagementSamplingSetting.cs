﻿//  
// Copyright (c) Microsoft.  All rights reserved.
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

namespace Microsoft.Azure.Commands.ApiManagement.ServiceManagement.Models
{
    public class PsApiManagementSamplingSetting
    {
        /// <summary>
        /// Gets or sets sampling type. Possible values include: 'fixed'
        /// </summary>
        public string SamplingType { get; set; }

        /// <summary>
        /// Gets or sets rate of sampling for fixed-rate sampling.
        /// </summary>
        public double? SamplingPercentage { get; set; }
    }
}