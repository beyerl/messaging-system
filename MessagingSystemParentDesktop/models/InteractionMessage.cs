using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingSystemParentDesktop.models
{
	public class InteractionMessage
	{
		public InteractionType Type { get; set; }
		
		public string Payload { get; set; }
	}
}
