using System;

namespace Lensert
{
    public class AccountEventArgs : EventArgs
    {
        public LensertClient LensertClient { get;  }

        public AccountEventArgs(LensertClient client)
        {
            LensertClient = client;
        }
    }
}