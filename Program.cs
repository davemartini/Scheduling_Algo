using System;
using System.Collections.Generic;

namespace Scheduling_Algo
{
    class Process_Intilizer
    {
        List<simuProcess> myProcesses;
        public Process_Intilizer()
        {
            myProcesses = new List<simuProcess>();
        }

        static void Main(string[] args)
        {
            Process_Intilizer algoGen = new();

            algoGen.clearAndFill();
            algoGen.printProcessInfo();
            
            algoGen.firstComeFirstServe();

            algoGen.clearAndFill();
            algoGen.shortestJobFirst();

            algoGen.clearAndFill();
            algoGen.priorityScheduling();

            algoGen.clearAndFill();
            algoGen.roundRobin();

            //algoGen.processBarPrinter("P1",5);

            //TODO: Implement ATT and AWT
        }
        public void clearAndFill()
        {
            myProcesses.Clear();
                                       //name, burst time, arrival time, priority, lower priority value goes first ie. 1 before 2 and 2 before 3 etc. 
            simuProcess process1 = new("P1", 2, 5, 3);
            simuProcess process2 = new("P2", 2, 0, 1);
            simuProcess process3 = new("P3", 4, 6, 4);
            simuProcess process4 = new("P4", 19,0, 5);
            simuProcess process5 = new("P5", 16, 10, 2);

            processStuffer(process1);
            processStuffer(process2);
            processStuffer(process3);
            processStuffer(process4);
            processStuffer(process5);
        }
        public void printProcessInfo()
        {
            Console.WriteLine("Current Process Information");
            Console.WriteLine("Name | Burst Time | Arrival Time | Priority Value");

            foreach(simuProcess currentProcess in myProcesses)
            {

                Console.WriteLine(currentProcess.pid+"        "+currentProcess.burstTime+"               "+currentProcess.arrivalTime+"          "+currentProcess.priorityValue);
            }
            Console.WriteLine("");
        }

        private void processStuffer(simuProcess passedProcess)
        {
            this.myProcesses.Add(passedProcess);
        }

        public void firstComeFirstServe()
        {
            //lambda expression uses arrivalTime for comparrison delegate
            myProcesses.Sort((x, y) => x.arrivalTime.CompareTo(y.arrivalTime));

            double timePassed = 0.0;
            double allWaitTimeSum = 0.0;
            double allTurnAroundTime = 0.0;

            Console.WriteLine("First Come First Serve Results");

            foreach (simuProcess currentProcess in myProcesses)
            {
                double finishingTime = currentProcess.burstTime + timePassed;
                currentProcess.turnAroundTime = finishingTime - currentProcess.arrivalTime;

                

                allTurnAroundTime = allTurnAroundTime + currentProcess.turnAroundTime;

                timePassed = timePassed + currentProcess.burstTime;

                processBarPrinter(currentProcess.pid, currentProcess.burstTime, timePassed) ;

            }

            foreach (simuProcess currentProcess in myProcesses)
            {
                currentProcess.waitTime = (currentProcess.turnAroundTime - currentProcess.burstTime);

                allWaitTimeSum = allWaitTimeSum+ currentProcess.waitTime; //summing wait time
            }
            Console.WriteLine("");
            foreach (simuProcess currentProcess in myProcesses)
            {

                //Console.WriteLine(currentProcess.pid + " turn around time is: " + currentProcess.turnAroundTime + " wait time is: " + currentProcess.waitTime);
            }
            
            Console.WriteLine("Average Turn Around time: {0}", (allTurnAroundTime / myProcesses.Count));
            Console.WriteLine("Average Wait Time: {0}\n", (allWaitTimeSum / myProcesses.Count));
        }

        public void shortestJobFirst()
        {
            Console.WriteLine("Shortest Job First Results");

            myProcesses.Sort((x, y) => x.burstTime.CompareTo(y.burstTime));

            double timePassed = 0.0;

            double totalTurnAroundTime = 0.0;
            double totalWaitTime = 0.0;

            for (double i = 0; i <= myProcesses.Count; i++)
            {
                foreach (simuProcess currentProcess in myProcesses)
                {
                    if (currentProcess.arrivalTime <= timePassed && currentProcess.finished == false)
                    {
                        double finishingTime = currentProcess.burstTime + timePassed;
                        currentProcess.turnAroundTime = finishingTime - currentProcess.arrivalTime;

                        totalTurnAroundTime = totalTurnAroundTime + currentProcess.turnAroundTime;

                        timePassed = timePassed + currentProcess.burstTime;
                        currentProcess.finished = true;

                        currentProcess.waitTime = (currentProcess.turnAroundTime - currentProcess.burstTime);
                        totalWaitTime = totalWaitTime + currentProcess.waitTime;

                        processBarPrinter(currentProcess.pid, currentProcess.burstTime, timePassed);

                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Average Turn Around Time: {0}", (totalTurnAroundTime / myProcesses.Count));
            Console.WriteLine("Average Wait Time: {0}", (totalWaitTime / myProcesses.Count));
        }

        public void priorityScheduling()
        {
            Console.WriteLine("");
            Console.WriteLine("Priority Scheduling Results");

            myProcesses.Sort((x, y) => x.priorityValue.CompareTo(y.priorityValue));

            double timePassed = 0.0;
            int priorityLevel = 1;

            double totalTurnAroundTime = 0.0;
            double totalWaitTime = 0.0;

            for (double i = 0; i <= myProcesses.Count; i++)
            {
                foreach (simuProcess currentProcess in myProcesses)
                {
                    if (currentProcess.arrivalTime <= timePassed && currentProcess.finished == false )
                    {
                        double finishingTime = currentProcess.burstTime + timePassed;
                        currentProcess.turnAroundTime = finishingTime - currentProcess.arrivalTime;
                        timePassed = timePassed + currentProcess.burstTime;
                        currentProcess.finished = true;
                        priorityLevel++;

                        currentProcess.waitTime = (currentProcess.turnAroundTime - currentProcess.burstTime);

                        processBarPrinter(currentProcess.pid, currentProcess.burstTime, timePassed);
                        totalTurnAroundTime = totalTurnAroundTime + currentProcess.turnAroundTime;
                        totalWaitTime = totalWaitTime + currentProcess.waitTime;

                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Total Turn Around Time: {0}", ((totalTurnAroundTime) / myProcesses.Count));
            Console.WriteLine("Total Wait Time: {0}", (totalWaitTime / myProcesses.Count));
        }
        public void roundRobin()
        {
            Console.WriteLine("");
            Console.WriteLine("Round Robin Results: Time Quantum 4");
            double timePassed = 0.0;
            double timeQuantum = 4;
            int indexTrack = 0;

            double totalTurnAroundTime = 0.0;
            double totalWaitTime = 0.0;

            double[] burstTrack = new double[myProcesses.Count];

            foreach(simuProcess currentProcess in myProcesses) //store burst times of each process in index, index also corrolates with a specific process property
            {
                burstTrack[indexTrack] = currentProcess.burstTime;
                currentProcess.roundRobinProcessIndex = indexTrack;
                indexTrack++;
            }
            int processCount = myProcesses.Count;
            int checkPass = 0;

            while (true)
            {
                foreach (simuProcess myProcess in myProcesses)
                {
                    if (timePassed >= myProcess.arrivalTime)
                    {
                        myProcess.arrived = true;
                        int accessIndex = myProcess.roundRobinProcessIndex;
                        for (int i = 0; i < burstTrack.Length; i++) //traverses to each process burst time index
                        {
                            if (burstTrack[i] <= timeQuantum && burstTrack[i] > 0 && i == accessIndex) //if the process burst time is less than or equal to the time quantum, only the burst time is added to time passed
                            {
                                double burstRemain = (double)burstTrack[i];
                                timePassed = timePassed + burstTrack[i];
                                burstTrack[i] = 0;
                                foreach (simuProcess currentProcess in myProcesses)
                                {
                                    if (currentProcess.roundRobinProcessIndex == i)
                                    {
                                        currentProcess.finished = true;
                                        checkPass++;

                                        double finishingTime = timePassed; //since this if statement is only accesssed when burst time is depleted to zero,
                                                                           //by default the computed time passed is equal to the finishing time of the process
                                        currentProcess.turnAroundTime = finishingTime - currentProcess.arrivalTime;
                                        currentProcess.waitTime = currentProcess.turnAroundTime - currentProcess.burstTime;

                                        
                                        processBarPrinter(currentProcess.pid, burstRemain, timePassed);

                                        totalTurnAroundTime = (totalTurnAroundTime + currentProcess.turnAroundTime);
                                        totalWaitTime = (totalWaitTime + currentProcess.waitTime);

                                        //Console.WriteLine("Finished " + currentProcess.pid + " turn around time is: " + currentProcess.turnAroundTime + " wait time is: " + currentProcess.waitTime);

                                    }
                                }
                            }

                            if (burstTrack[i] > timeQuantum && i == accessIndex) //if the burst time is greater than the time quantum, 4 seconds is
                                                                                 //subtracted from the array index, and time quantum is added to time passed
                            {
                                burstTrack[i] = burstTrack[i] - timeQuantum;
                                timePassed = timePassed + timeQuantum;

                                processBarPrinter(myProcess.pid, timeQuantum, timePassed); 
                            }
                        }

                    }
                    if (myProcess.arrived == false)
                    {
                        continue;
                    }
                }

                if (checkPass == processCount)
                {
                        break;
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Average Turn Around Time: {0}", (totalTurnAroundTime / myProcesses.Count));
            Console.WriteLine("Average Wait Time: {0}", (totalWaitTime / myProcesses.Count));
        }

        public void processBarPrinter(string processName, double processTime, double timePassed)
        {
            
            Console.Write("[{0}",processName);
            for(double i = 0; i < processTime; i++)
            {
                
                Console.Write("#");
                
            }
            Console.Write("{0}]",timePassed);
            
            
        }
   
    }
}



class simuProcess
{
    public string pid { set; get; }
    public double burstTime { private set; get; }
    public double arrivalTime { private set; get; }
    public int priorityValue { private set; get; }
    public double turnAroundTime { set; get; }
    public double waitTime { set; get; }
    public bool finished { set; get; }
    public int roundRobinProcessIndex { set; get; }

    public bool arrived { set; get; }



    public simuProcess(string ppid, int pburstTime, double parrivalTime, int ppriorityValue)
    {
        pid = ppid;
        burstTime = pburstTime;
        arrivalTime = parrivalTime;
        priorityValue = ppriorityValue;
        finished = false;
        arrived = false;

    }
}






