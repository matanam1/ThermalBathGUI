using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ThermalBathGUI
{
    internal class TestController
    {
        private String projName;
        private double vcc;
        private List<Double> ie1List;
        private List<Double> ie2List;
        private List<Double> ie3List;
        private double tempLow;
        private double tempHigh;
        private double tempStep;
        public TDAU tdau1;
        public TDAU tdau2;
        private bool[] tdau1Units;
        private bool[] tdau2Units;
        private String user;

        public TestController()
        {
            this.projName = String.Empty;
            this.ie1List = new List<Double>();
            this.ie2List = new List<Double>();
            this.ie3List = new List<Double>();
            this.tdau1 = new TDAU();
            this.tdau2 = new TDAU();
            this.tdau1Units = new bool[4];
            this.tdau2Units = new bool[4];
            this.user = String.Empty;
        }

        public String getProjName()
        {
            return projName;
        }

        public void printTest()
        {
            Console.Write(""+
                "Project Name: " + projName +
                "\nVcc :" + vcc +
                "\nIe1 currents: " + String.Join(", ", ie1List) +
                "\nIe2 currents: " + String.Join(", ", ie2List) +
                "\nIe3 corrents: " + String.Join(", ", ie3List) +
                "\nLowest temperuture: " +tempLow+
                "\nHighet tempruture: " +tempHigh+
                "\nstep between each tempruture: " +tempStep+
                "\nTDAU1: COM" + tdau1.getCom() + ", conection status: " + tdau1.getCnnectStatus()+
                "\nTDAU2: COM" + tdau2.getCom() + ", conection status: " + tdau2.getCnnectStatus() +
                "\nUser Email: " +user +"\n");
        }



        public void setProjName(String projName) {  this.projName = projName;  }
        public double getVcc() {  return vcc; }
        public void setVcc(double number) { this.vcc = number;  }
        public List<Double> getIe1List() { return ie1List; }
        public void clearIe1List() {  ie1List.Clear(); }
        public void addVarIe1List(Double ie) { this.ie1List.Add(ie); }
        public List<Double> getIe2List() { return ie2List; }
        public void addVarIe2List(Double ie) { this.ie2List.Add(ie); }
        public void clearIe2List()  { ie2List.Clear(); }
        public List<Double> getIe3List() { return ie3List; }
        public void addVarIe3List(Double ie) { this.ie3List.Add(ie); }
        public void clearIe3List() { ie3List.Clear(); }
        public double getTempLow() { return tempLow; }
        public void setTempLow(double number) { this.tempLow = number; }
        public double getTempHigh() { return tempHigh; }
        public void setTempHigh(double number) { this.tempHigh = number; }
        public double getTempStep() { return tempStep; }
        public void setTempStep(double number) { this.tempStep = number; }
        public int getCom1() { return tdau1.getCom(); }
        public void setCom1(int com) {
            tdau1.setCom(com);
        }
        public int getCom2() {return tdau2.getCom(); }
        public void setCom2(int com) { this.tdau2.setCom((com)); }
        public bool[] getTdau1Units() { return tdau1Units; }
        public void setTdau1Units(bool[] tdau1Units) { this.tdau1Units = tdau1Units; }
        public bool[] getTdau2Units() { return tdau2Units; }
        public void setTdau2Units(bool[] tdau2Units) { this.tdau2Units = tdau2Units;  }
        public String getUser() { return user; }
        public void setUser(String user) { this.user = user;  }

        public List<Double> getIeList(String ieType)
        {
            if (ieType.Equals("ie1"))
            {
                return ie1List;
            }
            else if (ieType.Equals("ie2"))
            {
                return ie2List;
            }
            else if (ieType.Equals("ie3"))
            {
                return ie3List;
            }
            else
            {
                // Handle other cases or throw an exception if needed
                return new List<double>(); // Return an empty list if no match is found
            }
        }

    }
}