using System;
using System.Collections;

namespace Sakuno.UserInterface.Internal
{
    class CollectionTeaser
    {
        Action<object> r_Add, r_Remove;

        public CollectionTeaser(Action<object> rpAdd, Action<object> rpRemove)
        {
            r_Add = rpAdd;
            r_Remove = rpRemove;
        }

        public void Add(object rpItem) => r_Add(rpItem);
        public void Remove(object rpItem) => r_Remove(rpItem);

        public static bool TryCreate(object rpCollection, out CollectionTeaser ropCollectionTeaser)
        {
            ropCollectionTeaser = null;

            var rList = rpCollection as IList;
            if (rList != null)
                ropCollectionTeaser = new CollectionTeaser(r => rList.Add(r), rList.Remove);
            else
            {

            }

            return ropCollectionTeaser != null;
        }
    }
}
