using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;



namespace ThermalBathGUI
{
    internal class TestController
    {
        private int projId;
        private String projName;
        private String projStep;
        private String user;
        private String email;


        private double vcc;
        private List<Double> ie1List;
        private List<Double> ie2List;
        private List<Double> ie3List;
        private double tempLow;
        private double tempHigh;
        private double tempStep;
        public List<TDAU> tdauList;


        public TestController()
        {
            this.projName = String.Empty;
            this.projStep = String.Empty;
            this.ie1List = new List<Double>();
            this.ie2List = new List<Double>();
            this.ie3List = new List<Double>();
            this.tdauList = new List<TDAU>();
            this.user = String.Empty;

            this.projId = -1;
            this.projStep = String.Empty;
            this.email = String.Empty;

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
                "\nstep between each tempruture: " + tempStep+
                "\nUser Email: " +user +"\n");
            foreach (var tdau in tdauList)
            {
                Console.Write("TDAU" + tdau.getSerialNumber() + ": COM" + tdau.getCom() + " , conection status: " + tdau.getCnnectStatus() + "\n");
            }
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

        public void addTdau(TDAU tdau)
        {
            tdauList.Add(tdau);
        }

        public TDAU getTdauByCom(int com)
        {
            foreach(TDAU tdau in tdauList)
            {
                if(tdau.getCom() == com)
                {
                    return tdau;
                }
            }
            return null;
        }

        public String getUser() { return user; }
        public void setUser(String user) { this.user = user;  }

        public String getEmail() {  return email; }
        public void setEmail(String email) {  this.email = email; }

        public String getProjStep() { return projStep; }
        public void setProjStep(String projStep) { this.projStep = projStep; }

        public int getProjId() {  return projId; }
        public void setProjId(int ProjId) { this.projId = ProjId; }




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

        public void disconnect()
        {
            foreach (var tdau in tdauList)
            {
                tdau.disconnect();
            }
            foreach (var tdau in tdauList)
            {
                tdau.disconnectPy();
            }
            foreach (var tdau in tdauList)
            {
                tdauList.Remove(tdau);
            }

        }


    }
}