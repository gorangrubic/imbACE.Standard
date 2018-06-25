namespace imbACE.Network.tools
{
    public static class imbWhoIsAnalysis
    {
        /*
        /// <summary>
        /// Vraca customServer url ako je pronadjen
        /// </summary>
        /// <param name="input"></param>
        /// <param name="myResult"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static String analyseWhoIsLines(List<String> input, imbScriptTaskResult myResult, String targetPath)
        {
            String customServer = "";
            Boolean end = false;
            int index = 0;
            int len = input.Count();
            String currentLine;
            while (!end)
            {
                Boolean analyze = true;
                currentLine = input[index];

                if (currentLine.IndexOf("NOTICE:") > -1) { analyze = false; end = true; }
                if (currentLine.IndexOf("TERMS OF USE:") > -1) { analyze = false; end = true; }

                if (currentLine.IndexOf("Whois Server Version") > -1)
                {
                    analyze = false;
                    myResult.log(targetPath + "/ServerVersion", currentLine, dataLogic.set);
                }

                if (currentLine.IndexOf(">>>") > -1)
                {
                    analyze = false;
                }

                if (analyze && (currentLine.IndexOf(":") > -1)) {
                    String[] tmpArr = currentLine.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    String key = tmpArr[0].Trim();
                    String value = tmpArr[1].Trim();

                    switch (key) {
                        case "Registrar":
                            myResult.log(targetPath + "/Registrar/Company", value, dataLogic.set);
                            break;

                        case "Whois Server":
                            myResult.log(targetPath + "/Registrar/WhoIsUrl", value, dataLogic.set);
                            customServer = value;
                            break;

                        case "Referral URL":
                            myResult.log(targetPath + "/Registrar/Url", value, dataLogic.set);
                            break;

                        case "Status":
                            myResult.log(targetPath + "/Status", value, dataLogic.set);
                            break;

                        case "Updated Date":
                            myResult.log(targetPath + "/Renewed", value, dataLogic.set);
                            break;

                        case "Creation Date":
                            myResult.log(targetPath + "/Created", value, dataLogic.set);
                            break;

                        case "Expiration Date":
                            myResult.log(targetPath + "/Expiration", value, dataLogic.set);
                            break;

                        default:
                            myResult.log(targetPath + "/Rest/Unhandled_"+key, value, dataLogic.set);
                            break;
                    }
                }

                index++;
                if (index > len)
                {
                    end = true;
                }
            }

            return customServer;
        }
         * */
    }
}