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

        public static bool TryCreate(object rpCollection, Type rpItemType, out CollectionTeaser ropCollectionTeaser)
        {
            ropCollectionTeaser = null;

            if (rpCollection == null)
                return false;

            var rList = rpCollection as IList;
            if (rList != null)
            {
                var rElementType = rList.GetType().GetGenericArguments()?[0];
                if (rElementType == null || !rpItemType.IsSubclassOf(rElementType))
                    return false;

                if (rList.IsReadOnly)
                    return false;

                ropCollectionTeaser = new CollectionTeaser(r => rList.Add(r), rList.Remove);
            }

            return ropCollectionTeaser != null;
        }
    }
}
