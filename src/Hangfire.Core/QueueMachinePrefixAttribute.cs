// This file is part of Hangfire.
// Copyright © 2013-2014 Sergey Odinokov.
// 
// Hangfire is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as 
// published by the Free Software Foundation, either version 3 
// of the License, or any later version.
// 
// Hangfire is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public 
// License along with Hangfire. If not, see <http://www.gnu.org/licenses/>.

using System;
using Hangfire.Common;
using Hangfire.States;
using System.Text.RegularExpressions;

namespace Hangfire
{   
    /// <summary>
    /// Prepends the machine name to the queue, so that other servers don't eat your queue jobs.
    /// Useful in debug scenarios where you want to consume your own jobs.
    /// </summary>
    public sealed class QueueMachinePrefixAttribute : JobFilterAttribute, IElectStateFilter
    {

        public static string MachineName = Regex.Replace(Environment.MachineName.ToLower(), "[^a-zA-Z0-9]", string.Empty);

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueMachinePrefixAttribute"/> class that prepends machine name to the queue name
        /// </summary>
        public QueueMachinePrefixAttribute()
        {
        }
        
        public void OnStateElection(ElectStateContext context)
        {
            var enqueuedState = context.CandidateState as EnqueuedState;
            if (enqueuedState != null)
            {
                enqueuedState.Queue += $"_{MachineName}";
            }
        }
    }
}
