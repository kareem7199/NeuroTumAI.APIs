using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Chat_Aggregate;

namespace NeuroTumAI.Core.Specifications
{
	public class ConversationSpecs : BaseSpecifications<Conversation>
	{
		public ConversationSpecs(string firstUserId, string secondUserId)
			: base(C => (C.FirstUserId == firstUserId && C.SecondUserId == secondUserId) || (C.FirstUserId == secondUserId && C.SecondUserId == firstUserId))
		{

		}
	}
}
