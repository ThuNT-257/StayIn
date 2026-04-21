using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._StayIn.Scripts.Definitions {

    [Serializable]
    public class ResourceItem {
        public ItemData itemData;
        public int quantity;
    }

    [Serializable]
    public struct DayActionData {
        public CharacterData character;
        public bool isFed;
        public bool isWatered;
        public bool isHealed;
    }
}
