namespace PMS.Droid.Classes
{
    public class HelperMethods
    {
        public static void Calculate(double total, int Duration, double intdue, double loandue,
         double principalLoan, double loanRepaid, double DurationOverLoanDue, double DurationOverIntDue,
         int InstallmentNo, double CumInterestPaid, double CumIntCharge, string calcMethod, out double vLoanInstallment, out double vInterestInstallment,int vDOC
         )
        {
            double vcumInrerestCharge;
            double vcumInrerestPaid;
            vcumInrerestCharge = CumIntCharge;
            vcumInrerestPaid = CumInterestPaid;
            double vInterestBalance = (vcumInrerestCharge - vcumInrerestPaid);

            vLoanInstallment = 0;
            vInterestInstallment = 0;
            double vPrincipalLOan = principalLoan;
            double vloanRepaid = loanRepaid;
            double vLoan = DurationOverLoanDue;
            double vInt = DurationOverIntDue;
            double vLoanDueSCase = DurationOverLoanDue;
            double vIntDueSCase = DurationOverIntDue;
            double vTotalInstall = (vLoan + vInt);
            if (InstallmentNo > Duration)
            {
                vLoan = DurationOverLoanDue;
                vInt = DurationOverIntDue;
                vLoanDueSCase = DurationOverLoanDue;
                vIntDueSCase = DurationOverIntDue;
                vTotalInstall = vLoan + vInt;
                if (total == 0)
                {
                    vLoanInstallment = 0;
                    vInterestInstallment = 0;
                }
                else
                {
                    if (calcMethod == "D")
                    {
                        /////////////////////For LOan And Int equal  0////////////////////////
                        if ((vPrincipalLOan - vloanRepaid) >= total)
                            vLoanInstallment = total;
                        if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
                            vLoanInstallment = (vPrincipalLOan - vloanRepaid);
                        if ((vPrincipalLOan - vloanRepaid) == 0)
                            vLoanInstallment = 0;

                      
                        if (vLoan == 0 && vInt > 0)
                        {
                            if (total >= vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInt)
                            {
                                vLoanInstallment = 0;
                            }
                        }
                        if (vDOC == 0)
                        {
                            if (vLoan > 0)
                            {

                                vLoanInstallment = (vLoan * total) / vTotalInstall;

                            }
                            else
                            {
                                if ((vPrincipalLOan - vloanRepaid) >= total)
                                    vLoanInstallment = total;
                                if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
                                    vLoanInstallment = (vPrincipalLOan - vloanRepaid);
                                if ((vPrincipalLOan - vloanRepaid) == 0)
                                    vLoanInstallment = 0;
                            }
                        }

                    }
                    else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "D")
                    {


                        if ((vPrincipalLOan - vloanRepaid) >= total)
                        {

                            vLoanInstallment = total;
                        }
                        if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
                        {
                            vLoanInstallment = (vPrincipalLOan - vloanRepaid);

                        }
                        if ((vPrincipalLOan - vloanRepaid) == 0)

                            vLoanInstallment = 0;
                        if (vDOC == 0)
                        {
                            if (vLoan > 0)
                            {

                                vLoanInstallment = (vLoan * total) / vTotalInstall;

                            }
                            else
                            {
                                if ((vPrincipalLOan - vloanRepaid) >= total)
                                {

                                    vLoanInstallment = total;
                                }
                                if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
                                {
                                    vLoanInstallment = (vPrincipalLOan - vloanRepaid);

                                }
                                if ((vPrincipalLOan - vloanRepaid) == 0)

                                    vLoanInstallment = 0;
                            }
                        }


                    }
                    else if (calcMethod == "E")
                    {

                        if (total > vInt)
                        {
                            vLoanInstallment = (total - vInt);
                        }
                        if (total <= vInt)
                        {
                            vLoanInstallment = 0;
                        }
                    }
                    else
                    {

                        /////////////////////For LOan And Int equal  0////////////////////////

                        if (vLoanDueSCase == 0 && vIntDueSCase > 0)
                        {
                            if (total >= vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vIntDueSCase)
                            {
                                vLoanInstallment = 0;
                            }
                        }
                        else if (vLoanDueSCase == 0 && vIntDueSCase == 0)
                        {
                            if (total > vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInterestBalance)
                            {
                                vLoanInstallment = 0;
                            }
                        }
                        /////////////////////for General Calculation///////////////////////////////////////
                        else if (vLoan > 0 && vInt > 0)
                        {
                            vLoanInstallment = (vLoan * total) / vTotalInstall;
                        }
                        else
                        {

                            vLoanInstallment = (vLoan * total) / vTotalInstall;
                        }
                    }

                }
                // loanPaidId = vLoanInstallment;

                if (total == 0)
                {

                    vInterestInstallment = 0;
                }
                else
                {
                    if (calcMethod == "D")
                    {

                        /////////////////////For LOan And Int equal  0////////////////////////
                        if ((vPrincipalLOan - vloanRepaid) >= total)
                        {

                            vInterestInstallment = 0;
                        }
                        //if ((parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) > 0 && (parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) < (parseFloat(vLoanDueSCase) + parseFloat(vIntDueSCase)))
                        if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
                        {
                            vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);

                        }
                        if ((vPrincipalLOan - vloanRepaid) == 0)

                            vInterestInstallment = total;

                  
                        if (vLoan == 0 && vInt > 0)
                        {
                            if (total >= vInterestBalance)
                            {
                                vInterestInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInt)
                            {
                                vInterestInstallment = 0;
                            }
                        }
                        if (vDOC == 0)
                        {
                            if (vTotalInstall > 0)
                            {

                                vInterestInstallment = (vInt * total) / vTotalInstall;

                            }
                            else
                            {
                                if ((vPrincipalLOan - vloanRepaid) >= total)
                                {

                                    vInterestInstallment = 0;
                                }
                                //if ((parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) > 0 && (parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) < (parseFloat(vLoanDueSCase) + parseFloat(vIntDueSCase)))
                                if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
                                {
                                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);

                                }
                                if ((vPrincipalLOan - vloanRepaid) == 0)

                                    vInterestInstallment = total;
                            }
                        }

                    }
                    else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "D")
                    {


                        if ((vPrincipalLOan - vloanRepaid) >= total)
                        {

                            vInterestInstallment = 0;
                        }
                        //if ((parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) > 0 && (parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) < (parseFloat(vLoanDueSCase) + parseFloat(vIntDueSCase)))
                        if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
                        {
                            vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);

                        }
                        if ((vPrincipalLOan - vloanRepaid) == 0)

                            vInterestInstallment = total;

                        if (vDOC == 0)
                        {
                            if (vTotalInstall > 0)
                            {

                                vInterestInstallment = (vInt * total) / vTotalInstall;

                            }
                            else
                            {
                                if ((vPrincipalLOan - vloanRepaid) >= total)
                                {

                                    vInterestInstallment = 0;
                                }
                                //if ((parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) > 0 && (parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) < (parseFloat(vLoanDueSCase) + parseFloat(vIntDueSCase)))
                                if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
                                {
                                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);

                                }
                                if ((vPrincipalLOan - vloanRepaid) == 0)

                                    vInterestInstallment = total;
                            }
                        }

                    }
                    else if (calcMethod == "E")
                    {
                        if (total > vInt)
                        {
                            vInterestInstallment = vInt;
                        }
                        if (total <= vInt)
                        {
                            vInterestInstallment = total;
                        }
                    }
                    else
                    {
                        /////////////////////For LOan And Int equal  0////////////////////////
                        if (vLoanDueSCase == 0 && vIntDueSCase > 0)
                        {
                            if (total >= vInterestBalance)
                            {
                                vInterestInstallment = vInterestBalance;
                            }
                            if (total <= vIntDueSCase)
                            {
                                vInterestInstallment = total;
                            }
                        }
                        else if (vLoanDueSCase == 0 && vIntDueSCase == 0)
                        {
                            if (total > vInterestBalance)
                            {
                                vInterestInstallment = vInterestBalance;
                            }
                            if (total <= vInterestBalance)
                            {
                                vInterestInstallment = total;
                            }
                        }
                        else if (vLoan > 0 && vInt > 0)
                        {
                            vInterestInstallment = (vInt * total) / vTotalInstall;
                        }
                        /////////////////////for General Calculation///////////////////////////////////////
                        else
                        {
                            vInterestInstallment = (vInt * total) / vTotalInstall;
                        }
                    }
                }
                double vLoanTotal = total;
                double vLoanBal;
                if (calcMethod != "A")
                {

                    double vCheck = (vloanRepaid + vLoanInstallment);
                    if (vCheck > vPrincipalLOan)
                    {
                        vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
                        vLoanInstallment = (vPrincipalLOan - vloanRepaid);

                    }

                    vLoanBal = (vPrincipalLOan + vInterestBalance - vloanRepaid);
                    double calIns = (vloanRepaid + vLoan);
                    if (vLoan >= vLoanBal)
                    {
                        vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
                        vLoanInstallment = (vPrincipalLOan - vloanRepaid);

                    }

                }
                else if (calcMethod == "A" || calcMethod == "R")
                {

                    vLoanBal = vPrincipalLOan - vloanRepaid;
                    var calIns = vInterestBalance + vLoanBal;
                    if (vLoan >= calIns)
                    {
                        if (calcMethod == "F")
                        {
                            vInterestInstallment = vInterestBalance;
                        }
                        else
                            vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);


                    }
                }


                var vLoanBalance = vPrincipalLOan - vloanRepaid;
                var vBal = vLoanBalance + vInterestBalance;
                if (vBal <= total)
                {

                    if (vInterestBalance > 0)
                    {
                        if (calcMethod == "F")
                        {
                            vInterestInstallment = vInterestBalance;
                        }
                        else
                            vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);



                    }
                    else

                        if (total > vLoanBalance)
                    {
                        if (calcMethod == "F")
                        {
                            vInterestInstallment = vInterestBalance;
                        }
                        else
                            vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
                    }
                    else
                    {
                        vInterestInstallment = 0;
                        vLoanInstallment = total;
                    }


                }
                if ((vLoanInstallment + vInterestInstallment) > total)
                {
                    vLoanInstallment = total - vInterestInstallment;

                }


                if (calcMethod == "F")
                {
                    if (total > vLoanInstallment + vInterestInstallment)
                    {
                        vLoanInstallment = 0;
                        vInterestInstallment = 0;
                        total = 0;

                    }
                }
            }

            else
            {
                vLoan = loandue;
                vInt = intdue;
                vTotalInstall = vLoan + vInt;
                if (total == 0)
                {
                    vLoanInstallment = 0;
                    vInterestInstallment = 0;
                }
                else
                {

                    if (calcMethod == "D")
                    {
                        /////////////////////For LOan And Int equal  0////////////////////////

                        if (vLoan == 0 && vInt > 0)
                        {

                            if (total > vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInterestBalance)
                            {
                                vLoanInstallment = 0;
                            }

                        }
                        else if (vLoan == 0 && vInt == 0)
                        {
                            if (total > vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInterestBalance)
                            {
                                vLoanInstallment = 0;
                            }
                        }

                        /////////////////////for General Calculation///////////////////////////////////////
                        else
                        {

                            if (total < vLoan)
                            {
                                vLoanInstallment = (vLoan * total) / vTotalInstall;
                                // vLoanInstallment = 0;
                            }
                            else
                            {
                                vLoanInstallment = (vLoan * total) / vTotalInstall;
                            }
                        }
                    }

                    else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "V")
                    {

                        /////////////////////For LOan And Int equal  0////////////////////////

                        if (vLoan == 0 && vInt > 0)
                        {
                            if (total > vInt)
                            {
                                vLoanInstallment = (total - vInt);
                            }
                            if (total <= vInt)
                            {
                                vLoanInstallment = 0;
                            }
                        }
                        else if (vLoan == 0 && vInt == 0)
                        {
                            if (total > vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInterestBalance)
                            {
                                vLoanInstallment = 0;
                            }
                        }
                        /////////////////////for General Calculation///////////////////////////////////////
                        else
                        {
                            if (calcMethod == "A")
                            {
                                if (vInterestBalance > vInt)
                                {
                                    if (total > vInterestBalance)
                                    {
                                        vLoanInstallment = (total - vInterestBalance);
                                    }
                                    if (total <= vInterestBalance)
                                    {
                                        vLoanInstallment = 0;
                                    }
                                    // vLoanInstallment = (parseFloat(total) - parseFloat(vInterestBalance))
                                }

                                else
                                {
                                    if (total > vInt)
                                    {
                                        vLoanInstallment = (total - vInt);
                                    }
                                    if (total <= vInt)
                                    {
                                        vLoanInstallment = 0;
                                    }
                                }
                            }
                            else if (calcMethod == "R" || calcMethod == "V")
                            {
                                if (total > (vLoan + vInt) && total - (vLoan + vInt) > vInterestBalance - vInt)
                                {
                                    if (vInterestBalance > 0)
                                    {
                                        vLoanInstallment = (total - vInterestBalance);

                                    }
                                    else
                                    {
                                        vLoanInstallment = total;
                                    }


                                }
                                else
                                {
                                    if (total < (vLoan + vInt))
                                    {

                                        if (total < vInt)
                                        {
                                            vLoanInstallment = 0;
                                        }
                                        else
                                        {
                                            vLoanInstallment = (total - vInt);
                                        }

                                    }
                                    else
                                        vLoanInstallment = vLoan;

                                }


                            }

                        }



                    }
                    else if (calcMethod == "E")
                    {
                        if (total > vInt)
                        {
                            vLoanInstallment = (total - vInt);
                        }
                        if (total <= vInt)
                        {
                            vLoanInstallment = 0;
                        }
                    }
                    else
                    {

                        /////////////////////For LOan And Int equal  0////////////////////////
                        if (vLoan == 0 && vInt > 0)
                        {
                            if (total >= vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInt)
                            {
                                vLoanInstallment = 0;
                            }
                        }
                        else if (vLoan == 0 && vInt == 0)
                        {
                            if (total > vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInterestBalance)
                            {
                                vLoanInstallment = 0;
                            }
                        }
                        /////////////////////for General Calculation///////////////////////////////////////
                        else
                        {

                            vLoanInstallment = (vLoan * total) / vTotalInstall;
                        }



                    }
                }

                if (total == 0)
                {

                    vInterestInstallment = 0;
                }
                else
                {
                    if (calcMethod == "D")
                    {
                        /////////////////////For LOan And Int equal  0////////////////////////

                        if (vLoan == 0 && vInt > 0)
                        {


                            if (total > vInterestBalance)
                            {
                                vInterestInstallment = vInterestBalance;
                            }
                            if (total <= vInterestBalance)
                            {
                                vInterestInstallment = total;
                            }
                        }
                        else if (vLoan == 0 && vInt == 0)
                        {
                            if (total > vInterestBalance)
                            {
                                vInterestInstallment = vInterestBalance;
                            }
                            if (total <= vInterestBalance)
                            {
                                vInterestInstallment = total;
                            }
                        }
                        /////////////////////for General Calculation///////////////////////////////////////
                        else
                        {


                            if (total < vInt)
                            {
                                vInterestInstallment = (vInt * total) / vTotalInstall;

                            }
                            else
                            {
                                vInterestInstallment = (vInt * total) / vTotalInstall;
                            }
                        }
                    }

                    else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "V")
                    {
                        /////////////////////For LOan And Int equal  0////////////////////////

                        if (vLoan == 0 && vInt > 0)
                        {
                            if (total > vInt)
                            {
                                vInterestInstallment = vInt;
                            }
                            if (total <= vInt)
                            {
                                vInterestInstallment = total;
                            }
                        }
                        else if (vLoan == 0 && vInt == 0)
                        {
                            if (total > vInterestBalance)
                            {
                                vInterestInstallment = vInterestBalance;
                            }
                            if (total <= vInterestBalance)
                            {
                                vInterestInstallment = total;
                            }
                        }
                        /////////////////////for General Calculation///////////////////////////////////////
                        else
                        {

                            if (calcMethod == "A")
                            {
                                if (vInterestBalance > vInt)
                                {
                                    if (total > vInterestBalance)
                                    {
                                        vInterestInstallment = vInterestBalance;
                                    }
                                    if (total <= vInterestBalance)
                                    {
                                        vInterestInstallment = total;
                                    }
                                    //vInterestInstallment = parseFloat(vInterestBalance)
                                }
                                else
                                {
                                    if (total > vInt)
                                    {
                                        vInterestInstallment = vInt;
                                    }
                                    if (total <= vInt)
                                    {
                                        vInterestInstallment = total;
                                    }
                                }
                            }
                            else if (calcMethod == "R" || calcMethod == "V")
                            {
                                if (total > (vLoan + vInt) && total - (vLoan + vInt) > (vInterestBalance - vInt))
                                {

                                    if (vInterestBalance > 0)
                                    {
                                        vInterestInstallment = vInterestBalance;
                                    }
                                    else
                                    {
                                        vInterestInstallment = 0;
                                    }


                                }
                                else
                                {
                                    if (total < (vLoan + vInt))
                                    {
                                        if (total < vInt)
                                        { vInterestInstallment = total; }
                                        else
                                        {
                                            vInterestInstallment = vInt;
                                        }

                                    }
                                    else
                                    {
                                        vInterestInstallment = (total - vLoan);
                                    }

                                }
                            }

                        }

                    }
                    else if (calcMethod == "E")
                    {
                        if (total > vInt)
                        {
                            vInterestInstallment = vInt;
                        }
                        if (total <= vInt)
                        {
                            vInterestInstallment = total;
                        }
                    }
                    else
                    {

                        /////////////////////For LOan And Int equal  0////////////////////////

                        if (vLoan == 0 && vInt > 0)
                        {
                            if (total >= vInterestBalance)
                            {
                                vInterestInstallment = vInterestBalance;
                            }

                            if (total < vInterestBalance)
                            {
                                vInterestInstallment = total;
                            }
                            if (total <= vInt)
                            {
                                vInterestInstallment = total;
                            }
                        }
                        else if (vLoan == 0 && vInt == 0)
                        {
                            if (total > vInterestBalance)
                            {
                                vInterestInstallment = vInterestBalance;
                            }
                            if (total <= vInterestBalance)
                            {
                                vInterestInstallment = total;
                            }
                        }
                        /////////////////////for General Calculation///////////////////////////////////////
                        else
                        {

                            vInterestInstallment = (vInt * total) / vTotalInstall;
                        }
                    }
                }

                if (calcMethod != "A")
                {
                    double vCheck = (vloanRepaid + vLoanInstallment);
                    if (vCheck > vPrincipalLOan)
                    {
                        if (calcMethod == "F")
                        {
                            vInterestInstallment = vInterestBalance;
                        }
                        else
                        {
                            vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);


                            vLoanInstallment = (vPrincipalLOan - vloanRepaid);
                        }

                    }

                    double vLoanBal = (vPrincipalLOan + vInterestBalance - vloanRepaid);
                    double calIns = (vloanRepaid + vLoan);
                    if (vLoan >= vLoanBal)
                    {
                        if (calcMethod == "F")
                        {
                            vInterestInstallment = vInterestBalance;
                            vLoanInstallment = (total - vInterestBalance);

                        }
                        else
                        {
                            vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
                            vLoanInstallment = (vPrincipalLOan - vloanRepaid);
                        }

                    }
                }
                else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "V")
                {
                    double vLoan1 = total;
                    double vLoanBal = vPrincipalLOan - vloanRepaid;
                    double calIns = (vloanRepaid + vLoan);
                    if (calIns >= vPrincipalLOan)
                    {
                        vLoanInstallment = (total - vInterestInstallment);

                    }
                }

                double vLoanBalance = vPrincipalLOan - vloanRepaid;
                double vBal = (vLoanBalance + vInterestBalance);
                if (vBal <= total)
                {

                    if (vInterestBalance > 0)
                    {
                        if (calcMethod == "F")
                        {
                            vInterestInstallment = vInterestBalance;
                            if ((total - vInterestBalance) >= (vPrincipalLOan - vloanRepaid))
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }


                        }
                        else
                        {
                            vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
                            vLoanInstallment = (vPrincipalLOan - vloanRepaid);
                        }


                    }
                    else
                        if (total > vLoanBalance)
                    {
                        if (calcMethod == "F")
                        {
                            vInterestInstallment = vInterestBalance;
                        }
                        else
                        {
                            vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
                            vLoanInstallment = (vPrincipalLOan - vloanRepaid);
                        }
                    }
                    else
                    {
                        vInterestInstallment = 0;
                        vLoanInstallment = total;
                    }


                }

                if ((vLoanInstallment + vInterestInstallment) > total)
                {
                    vLoanInstallment = total - vInterestInstallment;

                }
                if (calcMethod == "A" || calcMethod == "H")
                {


                    double vLoanPayable = (total - vInterestInstallment);
                    if (vLoanPayable > vPrincipalLOan)
                    {
                        double NeatLoanPay = total - vInterestBalance;
                        vLoanInstallment = NeatLoanPay;
                        double intPay = total - NeatLoanPay;
                        vInterestInstallment = intPay;
                    }
                    else
                    {
                        vLoanInstallment = vLoanPayable;

                    }

                }

                if (calcMethod == "F")
                {
                    if (vInterestInstallment > vInterestBalance)
                    {
                        vInterestInstallment = vInterestBalance;
                        vLoanInstallment = total - vInterestBalance;

                    }

                }




                if (calcMethod == "F")
                {
                    if (total > (vInterestBalance + vPrincipalLOan - vloanRepaid))
                    {
                        total = 0;
                        vLoanInstallment = 0;
                        vInterestInstallment = 0;


                    }
                }
            }
        }
    }
}