#define LOCALX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Wormholes
{
    class Program
    {
        static List<List<StarSystem>> starSystemCases = new List<List<StarSystem>>();
        static List<List<Wormhole>> wormholeCases = new List<List<Wormhole>>();
        static void Main(string[] args)
        {
            GetData();
            for(int caseInex = 0; caseInex < starSystemCases.Count; caseInex++)
            {
                starSystemCases[caseInex][0].Key = 0;
                int starSystemNum = starSystemCases[caseInex].Count;
                bool isChanged = true;
                int loopNum = 0;
                while(isChanged && loopNum < starSystemNum + 1)
                {
                    isChanged = false;
                    loopNum++;
                    foreach(Wormhole wormhole in wormholeCases[caseInex])
                    {
                        if(wormhole.TravelTo.Key > wormhole.TravelFrom.Key + wormhole.EndingUp)
                        {
                            wormhole.TravelTo.Key = wormhole.TravelFrom.Key + wormhole.EndingUp;
                            isChanged = true;
                            
                        }
                    }
                }

                if (isChanged)
                {
                    Console.WriteLine("possible");
                }
                else
                {
                    Console.WriteLine("not possible");
                }
            }
            

#if LOCAL
            Console.ReadLine();
#endif
        }
        static void GetData()
        {
#if LOCAL
            TextReader stdin = Console.In;
            Console.SetIn(new StreamReader("graph.txt"));
#endif
            string line = Console.ReadLine();
            int caseNum = int.Parse(line);
            string[] parts;
            int starSystemNum;
            int wormholeNum;
            List<StarSystem> starSystems;
            List<Wormhole> wormholes;
            for(int i = 0; i < caseNum; i++)
            {
                line = Console.ReadLine();
                parts = line.Split(' ');
                starSystemNum = int.Parse(parts[0]);
                wormholeNum = int.Parse(parts[1]);
                starSystems = new List<StarSystem>();
                for(int sn = 0; sn < starSystemNum; sn++)
                {
                    starSystems.Add(new StarSystem(sn));
                }
                starSystemCases.Add(starSystems);

                wormholes = new List<Wormhole>();
                for(int wn = 0; wn < wormholeNum; wn++)
                {
                    line = Console.ReadLine();
                    
                    parts = line.Split(' ');
                    int s1 = int.Parse(parts[0]);
                    int s2 = int.Parse(parts[1]);
                    int time = int.Parse(parts[2]);
                    wormholes.Add(new Wormhole(starSystems[s1], starSystems[s2], time));
                }
                wormholeCases.Add(wormholes);
            }

#if LOCAL
            Console.SetIn(stdin);
#endif
        }
    } // Class Program
    class StarSystem
    {
        public int ID { get; set; }
        public int Key { get; set; }
        public StarSystem(int id)
        {
            ID = id;
            Key = int.MaxValue;
        }
    }
    class Wormhole
    {
        public StarSystem TravelFrom { get; set; }
        public StarSystem TravelTo { get; set; }
        public int EndingUp { get; set; }
        public Wormhole(StarSystem travelFrom, StarSystem travelTo, int endingUp)
        {
            TravelFrom = travelFrom;
            TravelTo = travelTo;
            EndingUp = endingUp;
        }
    }
}
