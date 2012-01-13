﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace PRoCon.Core.Events {
    using Core.Variables;
    using Core.Remote;
    public class Expression {

         private PRoConClient m_prcClient;

        public string TextExpression {
            get;
            private set;
        }

        private List<string> Operators {
            get;
            set;
        }

        public Expression(PRoConClient prcClient, string strExpression) {
            this.TextExpression = strExpression;
            //this.m_prcClient = prcClient;

            this.Operators = new List<string>() { "/", "*", "-", "+", "%", "<", "<=", ">", ">=", "==", "!=", "&&", "||", "=" };
        }

        /// <summary>
        /// Converts "2 * (4 + 5) > 9" to "> * + 4 5 2 9"
        /// </summary>
        /// <returns></returns>
        private void CompileExpression() {



        }

        private object EvalOperator(string strOperator, string strLeft, string strRight) {
            object objReturn = null;

            double dblLeft = 0, dblRight = 0;
            bool blLeft = false, blRight = false;

            bool isNumeric = double.TryParse(strLeft, out dblLeft) && double.TryParse(strRight, out dblRight);
            bool isBoolean = bool.TryParse(strLeft, out blLeft) && bool.TryParse(strRight, out blRight);

            switch (strOperator) {
                case "/":
                    if (isNumeric == true) {
                        objReturn = (dblLeft / dblRight);
                    }
                    break;
                case "*":
                    if (isNumeric == true) {
                        objReturn = (dblLeft * dblRight);
                    }
                    break;
                case "-":
                    if (isNumeric == true) {
                        objReturn = (dblLeft - dblRight);
                    }
                    break;
                case "+":
                    if (isNumeric == true) {
                        objReturn = (dblLeft + dblRight);
                    }
                    break;
                case "%":
                    if (isNumeric == true) {
                        objReturn = (dblLeft % dblRight);
                    }
                    break;
                case "<":
                    if (isNumeric == true) {
                        objReturn = (dblLeft < dblRight);
                    }
                    break;
                case "<=":
                    if (isNumeric == true) {
                        objReturn = (dblLeft <= dblRight);
                    }
                    break;
                case ">":
                    if (isNumeric == true) {
                        objReturn = (dblLeft > dblRight);
                    }
                    break;
                case ">=":
                    if (isNumeric == true) {
                        objReturn = (dblLeft >= dblRight);
                    }
                    break;
                case "==":
                    if (isNumeric == true) {
                        objReturn = (dblLeft == dblRight);
                    }
                    else if (isBoolean == true) {
                        objReturn = (blLeft == blRight);
                    }
                    else {
                        objReturn = String.Compare(strLeft, strRight);
                    }
                    break;
                case "!=":
                    if (isNumeric == true) {
                        objReturn = (dblLeft != dblRight);
                    }
                    else if (isBoolean == true) {
                        objReturn = (blLeft != blRight);
                    }
                    break;
                case "&&":
                    if (isBoolean == true) {
                        objReturn = (blLeft && blRight);
                    }
                    break;
                case "||":
                    if (isBoolean == true) {
                        objReturn = (blLeft || blRight);
                    }
                    break;
                case "=":
                    Match mtcVariable;

                    if ((mtcVariable = Regex.Match(strLeft, "^procon\\.vars\\.(?<variable>.*)$", RegexOptions.IgnoreCase)).Success == true) {
                        if (mtcVariable.Groups.Count >= 2) {
                            this.m_prcClient.Variables.SetVariable(mtcVariable.Groups["variable"].Value, strRight);
                        }
                    }

                    objReturn = String.Empty;

                    break;
                default:
                    break;
            }

            return objReturn;
        }

        public T Evaluate<T>() {

            T tReturn = default(T);

            List<string> lstExpression = Packet.Wordify(this.TextExpression);

            while (lstExpression.Count >= 3) {

                object objOperatorResult = null;

                for (int i = 0; i < lstExpression.Count; i++) {

                    if (this.Operators.Contains(lstExpression[i]) == true && this.Operators.Contains(lstExpression[i + 1]) == false && this.Operators.Contains(lstExpression[i + 2]) == false) {

                        Match mtcVariable;

                        if (String.Compare(lstExpression[i], "=") != 0 && (mtcVariable = Regex.Match(lstExpression[i + 1], "^procon\\.vars\\.(?<variable>.*)$", RegexOptions.IgnoreCase)).Success == true) {
                            if (mtcVariable.Groups.Count >= 2) {
                                lstExpression[i + 1] = this.m_prcClient.Variables.GetVariable<string>(mtcVariable.Groups["variable"].Value, "");
                            }
                        }

                        if ((mtcVariable = Regex.Match(lstExpression[i + 2], "^procon\\.vars\\.(?<variable>.*)$", RegexOptions.IgnoreCase)).Success == true) {
                            if (mtcVariable.Groups.Count >= 2) {
                                lstExpression[i + 2] = this.m_prcClient.Variables.GetVariable<string>(mtcVariable.Groups["variable"].Value, "");
                            }
                        }
                        
                        objOperatorResult = this.EvalOperator(lstExpression[i], lstExpression[i + 1], lstExpression[i + 2]);

                        if (objOperatorResult != null) {
                            lstExpression.RemoveRange(i, 3);
                            lstExpression.Insert(i, Convert.ToString(objOperatorResult));
                        }
                        else {
                            throw new Exception(String.Format("Error in expression {0} {1} {2}", lstExpression[i], lstExpression[i + 1], lstExpression[i + 2]));
                        }
                    }
                }
            }

            TypeConverter tycPossible = TypeDescriptor.GetConverter(typeof(T));
            if (lstExpression[0].Length > 0 && tycPossible.CanConvertFrom(typeof(string)) == true) {

                tReturn = (T)tycPossible.ConvertFrom(lstExpression[0]);
            }

            return tReturn;
        }

    }

}
