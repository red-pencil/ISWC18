using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Klak.Wiring
{

	[NodeAttribute("TxKit/Transfer/Eye Instancer")]
	public class EyeInstancerNode : NodeBase {

		[Inlet]
		public Texture Image{
			set{
				if (!enabled) return;
			}
		}


		[Inlet]
		public Vector3 Position{
			set{
				if (!enabled) return;
			}
		}
	}
}
