using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCalc
{
    public class IPv4
    {
        private string ipAddress;
        private string mask;

        private string subnetAdd;
        private string wildcardMask;
        private string bCastAdd;
        private string addRange;
        private int slashMask;
        private double maxAdds;
        private int classOfNet;
        private double maxSubs;

        private byte[] ipAdds;
        private byte[] masks;
        private byte[] subnet;
        private byte[] wcMasks;
        private byte[] bCastAdds;

        public string SubnetAdd
        {
            get
            {
                return subnetAdd;
            }
        }
        public string AddRange
        {
            get
            {
                return addRange;
            }
        }
        public string WildcardMask
        {
            get
            {
                return wildcardMask;
            }
        }
        public string BroadcastAdd
        {
            get
            {
                return bCastAdd;
            }
        }
        public double MaxAdds
        {
            get
            {
                return maxAdds;
            }
        }
        public double MaxSubs
        {
            get
            {
                return maxSubs;
            }
        }

        public IPv4(string ipAddress, string mask)
        {
            this.ipAddress = ipAddress;
            this.mask = mask;
            SplitIp(out ipAdds, this.ipAddress);
            SplitIp(out masks, this.mask);
            FindSubnet();
            FindWildcardMask();
            FindBroadcastAdd();
            FindMask2();
            FindMaximumNumberOfAddresses();
        }

        public IPv4(string ipAddress, string mask, int classOfNet)
        {
            this.ipAddress = ipAddress;
            this.mask = mask;
            this.classOfNet = (classOfNet + 1) * 8;
            SplitIp(out ipAdds, this.ipAddress);
            SplitIp(out masks, this.mask);
            FindSubnet();
            FindWildcardMask();
            FindBroadcastAdd();
            FindMask2();
            FindMaximumNumberOfAddresses();
            FindMaximumNumberOfSubnets();
            FindAddressRange();
        }

        public void SetIp(string ipAddress)
        {
            this.ipAddress = ipAddress;
            SplitIp(out ipAdds, ipAddress);
            FindSubnet();
            FindBroadcastAdd();
        }
        
        public void SetMask(string mask)
        {
            this.mask = mask;
            SplitIp(out masks, mask);
            FindSubnet();
            FindWildcardMask();
            FindBroadcastAdd();
            FindMask2();
            FindMaximumNumberOfAddresses();
        }

        public void SetClassOfNet(int classOfNet)
        {
            this.classOfNet = classOfNet;
        }

        public void SetIpClassful(string ipAddress)
        {
            this.ipAddress = ipAddress;
            SplitIp(out ipAdds, ipAddress);
            FindSubnet();
            FindBroadcastAdd();
            FindAddressRange();
        }

        public void SetMaskClassful(string mask)
        {
            this.mask = mask;
            SplitIp(out masks, mask);
            FindSubnet();
            FindWildcardMask();
            FindBroadcastAdd();
            FindMask2();
            FindMaximumNumberOfAddresses();
            FindMaximumNumberOfSubnets();
            FindAddressRange();
        }

        #region Private methods to both Pages

        private void SplitIp(out byte[] ipObjects, string ipToSplit)
        {
            char[] delimiterChars = { '.', '/' };
            string[] ipStrings = ipToSplit.Split(delimiterChars);
            ipObjects = new byte[ipStrings.Length];

            for (int i = 0; i < ipStrings.Length; i++)
            {
                ipObjects[i] = Convert.ToByte(ipStrings[i]);
            }
        }

        private void FindSubnet()
        {
            subnetAdd = "";
            subnet = new byte[ipAdds.Length];

            for (int i = 0; i < subnet.Length; i++)
            {
                subnet[i] = (byte)(ipAdds[i] & masks[i]);
                subnetAdd += Convert.ToString(subnet[i]);
                if (i < subnet.Length - 1)
                {
                    subnetAdd += ".";
                }
            }
        }

        private void FindMask()
        {
            int res = slashMask / 8;
            int ost = slashMask % 8;

            for (int i = 1; i <= 4; i++)
            {
                if (i <= res)
                {
                    masks[i] = 255;
                }
                else
                {
                    switch (ost)
                    {
                        case 0:
                            masks[i] = 0;
                            break;
                        case 1:
                            masks[i] = 128;
                            break;
                        case 2:
                            masks[i] = 192;
                            break;
                        case 3:
                            masks[i] = 224;
                            break;
                        case 4:
                            masks[i] = 240;
                            break;
                        case 5:
                            masks[i] = 248;
                            break;
                        case 6:
                            masks[i] = 252;
                            break;
                        case 7:
                            masks[i] = 254;
                            break;
                    }
                }
            }
        }

        private void FindMask2()
        {
            slashMask = 0;

            foreach (byte msk in masks)
            {
                switch (msk)
                {
                    case 0:
                        slashMask += 0;
                        break;
                    case 128:
                        slashMask += 1;
                        break;
                    case 192:
                        slashMask += 2;
                        break;
                    case 224:
                        slashMask += 3;
                        break;
                    case 240:
                        slashMask += 4;
                        break;
                    case 248:
                        slashMask += 5;
                        break;
                    case 252:
                        slashMask += 6;
                        break;
                    case 254:
                        slashMask += 7;
                        break;
                    case 255:
                        slashMask += 8;
                        break;
                }
            }
        }

        private void FindWildcardMask()
        {
            wildcardMask = "";
            wcMasks = new byte[masks.Length];

            for (int i = 0; i < masks.Length; i++)
            {
                wcMasks[i] = (byte)(~masks[i]);
                wildcardMask += Convert.ToString(wcMasks[i]);
                if (i < masks.Length - 1)
                {
                    wildcardMask += ".";
                }
            }
        }

        private void FindBroadcastAdd()
        {
            bCastAdd = "";
            bCastAdds = new byte[subnet.Length];

            for (int i = 0; i < subnet.Length; i++)
            {
                bCastAdds[i] = (byte)(subnet[i] | wcMasks[i]);
                bCastAdd += Convert.ToString(bCastAdds[i]);
                if (i < subnet.Length - 1)
                {
                    bCastAdd += ".";
                }
            }
        }

        private void FindMaximumNumberOfAddresses()
        {
            maxAdds = Math.Pow(2, (32 - slashMask)) - 2;
            if (maxAdds < 0)
            {
                maxAdds = 0;
            }
        }

        #endregion

        #region Private methods to ClassfulPage

        private void FindMaximumNumberOfSubnets()
        {
            maxSubs = Math.Pow(2, (slashMask - classOfNet));
        }

        private void FindAddressRange()
        {
            int index = 0;
            addRange = "";
            byte[] rangeArr = new byte[4];

            for (int i = 0; i < subnet.Length; i++)
            {
                if (i < subnet.Length - 1)
                {
                    addRange += Convert.ToString(subnet[i]);
                    addRange += ".";
                }
                else
                {
                    addRange += Convert.ToString(subnet[i] + 1);
                }
            }

            addRange += " - ";

            for (int i = bCastAdds.Length - 1; i >= 0; i--)
            {
                if((bCastAdds[i] != 0) && (index == 0))
                {
                    rangeArr[i] = (byte)(bCastAdds[i] - 1);
                    index++;
                }
                else if(bCastAdds[i] != 0)
                {
                    rangeArr[i] = bCastAdds[i];
                    index++;
                }
                else
                {
                    rangeArr[i] = bCastAdds[i];
                }
            }

            for (int i = 0; i < bCastAdds.Length; i++)
            {
                addRange += Convert.ToString(rangeArr[i]);

                if (i < bCastAdds.Length - 1)
                {
                    addRange += ".";
                }
            }
        }

        #endregion
    }
}
