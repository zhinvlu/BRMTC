using AForge.Neuro;
//using AForge.Neuro.Learning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Index
{
    class Program
    {
        #region 全局变量
        const int OBJECTNUM = 48;
        const int IndexNUM = 10;//10个指标
        const int N = 100000;
        const int OBJECTNUM1 = 12;//12个省份
        const int LITTLE1 = 2;
        const int LITTLE2 = 3;
        const int YEAR = 4; //4年
        const int Input_layer = 10;
        const int Hidden_layer = 6;
        const int Output_layer = 1;
        const double Learning_Rate = 0.3;
        const double Momentum = 0.0;
        const bool needToStop = false;
        const int iterations = 1000;
        //const int samples = 48; //input样本数
        #endregion
        static void Main(string[] args)
        {
            #region 原始数据
            //陕甘宁青新广黑吉辽内云重
            //List<double> holeindex_all_year = new List<double>()
            //{
            //    33464,38564,42692,46929,          19525,22075,24539,26433,          33043,36394,39421,41834,        29522,33181,36875,39671,         30087,33796,37181,40648,          25326,27951,30741,33090,              32819,35711,37697,39226,              38460,43415,47428,50160,              50760,56649,61996,65201,                57974,59900,67836,71046,              19265,22195,25322,27264,            34500,38914,42795,47850,
            //    0.99,1.05,1.04,1.13,                      0.73,0.79,0.86,0.83,                     0.016,0.017,0.018,0.019,            0.004,0.006,0.007,0.008,             1.02,1.07,1.13,1.07,                     1.1,1.43,1.69,1.72,                           1.19,1.19,1.15,1.15,                         0.96,1.01,1.35,1.25,                          2.23,2.22,2.28,2.14,                           0.94,0.81,0.85,0.84,                         0.96,0.91,1.04,0.97,                       0.97,0.97,1.26,1.15,
            //    0.86,0.86,0.95,1.03,                     0.33,0.36,0.37,0.39,                      0.006,0.006,0.006,0.007,            0.01,0.01,0.009,0.009,                 0.02,0.03,0.06,0.05,                     0.48,0.52,0.56,0.52,                         2.38,2.53,2.38,2.56,                         1.04,1.07,0.98,1.01,                         1,1.05,1.09,1.1,                                  0.12,0.13,0.14,0.13,                         0.96,0.91,0.95,1.03,                        1.12,1.23,1.08,1.22,
            //    1.99,1.99,2.14,2.07,                     0.48,0.52,0.56,0.52,                      0.73,0.78,0.82,0.85,                   0.75,0.69,0.64,0.62,                     0.5,0.53,0.54,0.53,                       0.69,0.75,0.78,0.71,                        1.02,1.06,1.13,1.07,                          0.84,0.91,0.92,0.94,                         1.63,1.57,1.63,1.52,                           0.6,0.64,0.7,0.68,                             0.63,0.66,0.67,0.67,                       1.28,1.4,1.39,1.41,
            //    73501,82428,93494,97138,          21332,24290,25049,27124,          7359,8073,8234,9500,                5004,5181,4767,4731,                15440,15671,15822,15662,          40135,41268,40664,41208,              66599,65118,62660,62648,              44815,45749,48008,49774,              80977,87180,94885,99586,                27604,31819,37277,36435,              25092,27817,28483,30523,            40698,46112,52612,58354,
            //    11662,10034,12836,22820,          2383,3552,4737,5097,                 613,844,1211,1424,                     538,527,502,619,                        2642,3440,4998,5238,                 4402,5900,7884,9664,                    12236,20268,19819,15412,               4920,5923,6219,6696,                     19176,21216,21656,19525,                2262,3090,3836,4031,                    4199,5853,6804,8124,                   15525,20364,24828,24312,
            //    215.37,334.82,533.31,639.98,       52.64,73.06,99.99,114.52,            3.94,2.91,1.43,3.18,                     11.46,21.06,26.89,35.43,              2.783,2.857,2.965,3.162,              5.64,2.52,7.34,11.58,                     62.07,100.45,101.77,120.28,             26.3,25.1,34.7,28.2,                         159.7,230.7,180,250.9,                       73.42,218.43,158.93,157.67,            11.71,45.48,42,47.92,                    68.14,54.02,90.28,156.2,
            //    66997,64336,64353,64798,          3178,3342,3328,3868,                 8077,7881,7557,8452,                 2564,4195,3046,2742,                  20018,20382,20883,21228,         3178,3342,3328,3868,                   4626,3781,3835,3791,                      47362,36838,38422,39186,              47362,36838,37836,39186,                737,823,1005,948,                          3178,3342,3328,3868,                    34166,36697,36555,38405,
            //    384932,468407,331484,328561,   149820,260028,299229,290337,   148544,251093,237415,262545,  263606,275036,290417,307456,   226848,249202,265325,285958,  256677,355890,318323,294770,     220377,272229,242528,251080,        244299,313780,390827,311540,       244299,313780,390827,311540,        189534,263203,272421,275055,      209120,261167,279802,283468,      175332,261969,298643,303363,
            //    0.846,0.816,0.787,0.708,              1.28,1.21,1.15,1.1,                       2.79,2.66,2.51,2.43,                     2.44,2.47,2.41,1.78,                     1.5,1.57,1.63,1.65,                       0.68,0.65,0.63,0.6,                         0.8,0.73,0.67,0.62,                            0.91,0.84,0.72,0.67,                          0.68,0.63,0.55,0.51,                          1.41,1.33,1.27,1.18,                        1.09,1.01,0.97,0.94,                         0.95,0.89,0.77,0.66
            //};
            List<double> holeindex_all_year = new List<double>();
            List<double> holeindex = new List<double>();
            //List < double > outputall_2011 = new List<double>();
            double[][] input = new double[OBJECTNUM][];//神经网络input
            double[][] output = new double[OBJECTNUM][];//神经网络output
            List<List<double>> listGroup = new List<List<double>>();
            List<double> outputall = new List<double>();
            List<double> Zhi_Shu_List = new List<double>();
            //计算出熵值列表
            List<double> shang_list;
            //计算权重系数
            List<double> quan_zhong_xi_shu_list;
            int year = 2011;
            BackPropagationLearning teacher;
            List<double> r_ij;
            List<double> R_ij;
            List<double> S_ij;
            Dictionary<string, double> Zhi_Shu_Zi_Dian;
            List<double> Ji_Chu_Huan_Jing_List;
            List<double> Tou_Ru__List;
            List<double> Chan_Chu_List;
            List<double> Ji_Xiao_List;
            #endregion
            #region 计算大指数
            holeindex_all_year = ReadFileByLine("Data.txt");
            
            for (int i = 0; i < YEAR; i++)
            {
                holeindex = Split_All_Year_Data_To_Eachyear(holeindex_all_year, i);
                listGroup = list_fen_zu(holeindex, IndexNUM, OBJECTNUM1);
                outputall = Normalization(listGroup);
                listGroup = list_fen_zu(outputall, IndexNUM, OBJECTNUM1);
                Compute_Shang_List(listGroup, out shang_list);
                Quan_Zhong_xi_shu(shang_list, out quan_zhong_xi_shu_list);
                listGroup = list_fen_zu1(outputall, OBJECTNUM1);
                Zhi_Shu_List = Compute_ZhiShu_List(listGroup, quan_zhong_xi_shu_list);
                init_input_and_output_data(input, output, listGroup, Zhi_Shu_List, i);
                
                if (2011 != year)
                {
                    Console.WriteLine("\n");
                }
                Console.WriteLine("{0:D}年熵值法综合评价值：", year);
                init_dictionary(out Zhi_Shu_Zi_Dian, Zhi_Shu_List);
                PrintData(Zhi_Shu_Zi_Dian, Zhi_Shu_List);                
                year++;
            }

            #endregion
            #region 调用BP神经网络
            SearchSolution(out teacher,input, output);
            #endregion
            #region 神经网络计算出权重
            Compute_r_ij(out r_ij, teacher.weightsUpdates[0], teacher.weightsUpdates[1]);
            Compute_R_ij(out R_ij, r_ij);
            Compute_S_ij(out S_ij, R_ij);
            year = 2011;
            for (int i = 0; i < YEAR; i++)
            {
                holeindex = Split_All_Year_Data_To_Eachyear(holeindex_all_year, i);
                listGroup = list_fen_zu(holeindex, IndexNUM, OBJECTNUM1);
                outputall = Normalization(listGroup);
                listGroup = list_fen_zu(outputall, IndexNUM, OBJECTNUM1);
                //Compute_Shang_List(listGroup, out shang_list);
                //Quan_Zhong_xi_shu(shang_list, out quan_zhong_xi_shu_list);
                listGroup = list_fen_zu1(outputall, OBJECTNUM1);
                Zhi_Shu_List = Compute_ZhiShu_List(listGroup, S_ij);
                if (2011 != year)
                {
                    Console.WriteLine("\n");
                }
                Console.WriteLine("{0:D}年大指数：", year);
                init_dictionary(out Zhi_Shu_Zi_Dian, Zhi_Shu_List);
                PrintData(Zhi_Shu_Zi_Dian, Zhi_Shu_List);                
                year++;
            }
            #endregion
            #region 输出4年小指数和K-means
            year = 2011;
            for (int i = 0; i < YEAR; i++)
            {
                holeindex = Split_All_Year_Data_To_Eachyear(holeindex_all_year, i);
                listGroup = list_fen_zu(holeindex, IndexNUM, OBJECTNUM1);
                outputall = Normalization(listGroup);
                listGroup = list_fen_zu(outputall, IndexNUM, OBJECTNUM1);
                //Compute_Shang_List(listGroup, out shang_list);
                //Quan_Zhong_xi_shu(shang_list, out quan_zhong_xi_shu_list);
                JI_CHU_HUAN_JING(listGroup, outputall, S_ij, year, out Zhi_Shu_Zi_Dian,out Ji_Chu_Huan_Jing_List);
                TOU_RU(listGroup, outputall, S_ij, year, out Zhi_Shu_Zi_Dian, out Tou_Ru__List);
                CHAN_CHU(listGroup, outputall, S_ij, year, out Zhi_Shu_Zi_Dian, out Chan_Chu_List);
                JI_XIAO(listGroup, outputall, S_ij, year, out Zhi_Shu_Zi_Dian, out Ji_Xiao_List);
                K_MEANS(Ji_Chu_Huan_Jing_List, Tou_Ru__List, Chan_Chu_List, Ji_Xiao_List);
                year++;
            }
            #endregion
            #region 屏蔽掉的代码
            //#region 蒙特卡洛验证
            ////蒙特卡洛验证
            ////产生随机权重
            //List<double> Sui_Ji_Shu_List = new List<double>();
            //List<double> Jia_Quan_Zhi_List = new List<double>();
            //List<double> temp1 = new List<double>();
            //List<List<int>> Ge_Zhi_Shu_Wei_Zhi = new List<List<int>>();
            //for (int i = 0; i < OBJECTNUM1; i++)
            //{
            //    List<int> test = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //    Ge_Zhi_Shu_Wei_Zhi.Add(test);
            //}
            //for (int i = 0; i < N; i++)
            //{
            //    Sui_Ji_Shu_List = Chan_Sheng_Sui_Ji_Quan_Zhong(IndexNUM);
            //    foreach (var item in listGroup)
            //    {
            //        Jia_Quan_Zhi_List.Add(Zhi_Shu(Sui_Ji_Shu_List, item));
            //    }
            //    foreach (var item in Jia_Quan_Zhi_List)
            //    {
            //        //Console.WriteLine(item);
            //    }

            //    //temp1 = Jia_Quan_Zhi_List;
            //    Jia_Quan_Zhi_List.ForEach(q => temp1.Add(q));
            //    temp1.Sort();
            //    int shun_xu = 0;
            //    for (int j = 0; j < OBJECTNUM1; j++)
            //    {
            //        shun_xu = temp1.IndexOf(Jia_Quan_Zhi_List[j]);
            //        Ge_Zhi_Shu_Wei_Zhi[j][shun_xu] += 1;
            //    }
            //    temp1.Clear();
            //    Jia_Quan_Zhi_List.Clear();
            //}
            //Console.WriteLine("\n");
            //Console.WriteLine("蒙特卡洛验证:");
            //Dictionary<string, double> Meng_Te_Ka_Luo_Zi_Dian = new Dictionary<string, double>();
            //int PaiMing = 0;
            //for (int j = 0; j < OBJECTNUM1; j++)
            //{
            //    PaiMing = OBJECTNUM1 - Ge_Zhi_Shu_Wei_Zhi[j].IndexOf(Ge_Zhi_Shu_Wei_Zhi[j].Max());
            //    switch (j)
            //    {
            //        case 0:
            //            Meng_Te_Ka_Luo_Zi_Dian.Add("陕西", PaiMing);
            //            break;
            //        case 1:
            //            Meng_Te_Ka_Luo_Zi_Dian.Add("甘肃", PaiMing);
            //            break;
            //        case 2:
            //            Meng_Te_Ka_Luo_Zi_Dian.Add("宁夏", PaiMing);
            //            break;
            //        case 3:
            //            Meng_Te_Ka_Luo_Zi_Dian.Add("青海", PaiMing);
            //            break;
            //        case 4:
            //            Meng_Te_Ka_Luo_Zi_Dian.Add("新疆", PaiMing);
            //            break;
            //        case 5:
            //            Meng_Te_Ka_Luo_Zi_Dian.Add("广西", PaiMing);
            //            break;
            //        case 6:
            //            Meng_Te_Ka_Luo_Zi_Dian.Add("黑龙江", PaiMing);
            //            break;
            //        case 7:
            //            Meng_Te_Ka_Luo_Zi_Dian.Add("吉林", PaiMing);
            //            break;
            //        case 8:
            //            Meng_Te_Ka_Luo_Zi_Dian.Add("辽宁", PaiMing);
            //            break;
            //        case 9:
            //            Meng_Te_Ka_Luo_Zi_Dian.Add("内蒙", PaiMing);
            //            break;
            //        case 10:
            //            Meng_Te_Ka_Luo_Zi_Dian.Add("云南", PaiMing);
            //            break;
            //        case 11:
            //            Meng_Te_Ka_Luo_Zi_Dian.Add("重庆", PaiMing);
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //foreach (KeyValuePair<string, double> kvp in Meng_Te_Ka_Luo_Zi_Dian)
            //{
            //    Console.WriteLine("省份：{0},排名：{1}", kvp.Key, kvp.Value);
            //}
            //#endregion 
            #endregion            
            

        }
        #region k-means
        static void K_MEANS(List<double> Ji_Chu_Huan_Jing_List, List<double> Tou_Ru__List, List<double> Chan_Chu_List, List<double> Ji_Xiao_List)
        {
            //step1 定义样本变量（如陕西。云南等）
            List<Node> Yangben_arg = new List<Node>();  //数组， 存放12个省份的样本
            List<Node> clusters = new List<Node>();         //数组，存放均值
            int numberOfClusters = 4;                      //分几组
            //初始化样本list
            for (int i = 0; i < OBJECTNUM1; i++)
            {
                Node tempnode = new Node();
                tempnode.Ji_Chu_Huan_Jing = Ji_Chu_Huan_Jing_List[i];
                tempnode.Tou_Ru = Tou_Ru__List[i];
                tempnode.Chan_chu = Chan_Chu_List[i];
                tempnode.Ji_xiao = Ji_Xiao_List[i];
                Yangben_arg.Add(tempnode);
            }
            //初始化均值list
            for (int i = 0; i < numberOfClusters; i++)
            {
                Node tempnode = new Node();
                clusters.Add(tempnode);
            }


            bool _changed = true;
            bool _success = true;
            // 初始化质心
            InitializeCentroids(Yangben_arg, numberOfClusters);

            int maxIteration = Yangben_arg.Count * 10;
            int _threshold = 0;
            while (_success == true && _changed == true && _threshold < maxIteration)
            {
                ++_threshold;
                _success = UpdateNodeMeans(Yangben_arg, clusters);  //更新均值（Means）
                _changed = UpdateClusterMembership(numberOfClusters, Yangben_arg, clusters);
            }

            Dictionary<string, Node> K_Means_Zi_Dian = new Dictionary<string, Node>();
            K_Means_Zi_Dian.Add("陕西", Yangben_arg[0]);
            K_Means_Zi_Dian.Add("甘肃", Yangben_arg[1]);
            K_Means_Zi_Dian.Add("宁夏", Yangben_arg[2]);
            K_Means_Zi_Dian.Add("青海", Yangben_arg[3]);
            K_Means_Zi_Dian.Add("新疆", Yangben_arg[4]);
            K_Means_Zi_Dian.Add("广西", Yangben_arg[5]);
            K_Means_Zi_Dian.Add("黑龙江", Yangben_arg[6]);
            K_Means_Zi_Dian.Add("吉林", Yangben_arg[7]);
            K_Means_Zi_Dian.Add("辽宁", Yangben_arg[8]);
            K_Means_Zi_Dian.Add("内蒙", Yangben_arg[9]);
            K_Means_Zi_Dian.Add("云南", Yangben_arg[10]);
            K_Means_Zi_Dian.Add("重庆", Yangben_arg[11]);

            Console.WriteLine("------------------------------");
            Console.WriteLine("K-Means算法分组如下：");
            var group = Yangben_arg.GroupBy(s => s.Cluster).OrderBy(s => s.Key);
            foreach (var g in group)
            {
                Console.WriteLine("Cluster # " + g.Key + ":");
                foreach (var value in g)
                {
                    foreach (KeyValuePair<string, Node> kv in K_Means_Zi_Dian)
                    {

                        if (value.Equals(kv.Value))
                        {
                            Console.WriteLine("{0}", kv.Key);
                        }
                    }
                }
                Console.WriteLine("------------------------------");
            }
        }
        #endregion
        #region 小指数
        static void JI_CHU_HUAN_JING(List<List<double>> listGroup, List<double> outputall, List<double> S_ij,int year, out Dictionary<string, double> Zhi_Shu_Zi_Dian,out List<double> Ji_Chu_Huan_Jing_List)
        {

            listGroup.Clear();
            List<double> index1 = new List<double>(GetSubList(outputall, 0, 23));
            List<double> quan_zhong_xi_shu_list_1 = new List<double>();
            double temp_index_1 = S_ij[0] + S_ij[1];
            quan_zhong_xi_shu_list_1.Add(S_ij[0] / temp_index_1);
            quan_zhong_xi_shu_list_1.Add(S_ij[1] / temp_index_1);

            //Zhi_Shu_List.Clear();
            Ji_Chu_Huan_Jing_List = new List<double>();
            listGroup = list_fen_zu1(index1, OBJECTNUM1);
            foreach (var item in listGroup)
            {
                Ji_Chu_Huan_Jing_List.Add(Zhi_Shu(quan_zhong_xi_shu_list_1, item));
            }
            Console.WriteLine("\n");
            Console.WriteLine("{0:D}年小指数:基础环境:", year);
            init_dictionary(out Zhi_Shu_Zi_Dian, Ji_Chu_Huan_Jing_List);
            PrintData(Zhi_Shu_Zi_Dian, Ji_Chu_Huan_Jing_List);
        }
        static void TOU_RU(List<List<double>> listGroup, List<double> outputall, List<double> S_ij, int year, out Dictionary<string, double> Zhi_Shu_Zi_Dian,out List<double> Tou_Ru__List)
        {
            listGroup.Clear();
            List<double> index2 = new List<double>(GetSubList(outputall, 24, 59));
            List<double> quan_zhong_xi_shu_list_2 = new List<double>();
            double temp_index_2 = S_ij[2] + S_ij[3] + S_ij[4];
            quan_zhong_xi_shu_list_2.Add(S_ij[2] / temp_index_2);
            quan_zhong_xi_shu_list_2.Add(S_ij[3] / temp_index_2);
            quan_zhong_xi_shu_list_2.Add(S_ij[4] / temp_index_2);
            //Quan_Zhong_xi_shu(shang_list, out quan_zhong_xi_shu_list);

            //Zhi_Shu_List.Clear();
            Tou_Ru__List = new List<double>();
            listGroup = list_fen_zu1(index2, OBJECTNUM1);
            foreach (var item in listGroup)
            {
                Tou_Ru__List.Add(Zhi_Shu(quan_zhong_xi_shu_list_2, item));
            }
            Console.WriteLine("\n");
            Console.WriteLine("{0:D}年小指数:投入:", year);
            init_dictionary(out Zhi_Shu_Zi_Dian, Tou_Ru__List);
            PrintData(Zhi_Shu_Zi_Dian, Tou_Ru__List);
        }
        static void CHAN_CHU(List<List<double>> listGroup, List<double> outputall, List<double> S_ij, int year, out Dictionary<string, double> Zhi_Shu_Zi_Dian, out List<double> Chan_Chu_List)
        {
            listGroup.Clear();
            List<double> index3 = new List<double>(GetSubList(outputall, 60, 95));
            List<double> quan_zhong_xi_shu_list_3 = new List<double>();
            double temp_index_3 = S_ij[5] + S_ij[6] + S_ij[7];
            quan_zhong_xi_shu_list_3.Add(S_ij[5] / temp_index_3);
            quan_zhong_xi_shu_list_3.Add(S_ij[6] / temp_index_3);
            quan_zhong_xi_shu_list_3.Add(S_ij[7] / temp_index_3);

            //Zhi_Shu_List.Clear();
            Chan_Chu_List = new List<double>();
            listGroup = list_fen_zu1(index3, OBJECTNUM1);
            foreach (var item in listGroup)
            {
                Chan_Chu_List.Add(Zhi_Shu(quan_zhong_xi_shu_list_3, item));
            }
            Console.WriteLine("\n");
            Console.WriteLine("{0:D}年小指数:产出:", year);
            init_dictionary(out Zhi_Shu_Zi_Dian, Chan_Chu_List);
            PrintData(Zhi_Shu_Zi_Dian, Chan_Chu_List);
        }
        static void JI_XIAO(List<List<double>> listGroup, List<double> outputall, List<double> S_ij, int year, out Dictionary<string, double> Zhi_Shu_Zi_Dian, out List<double> Ji_Xiao_List)
        {
            listGroup.Clear();
            List<double> index4 = new List<double>(GetSubList(outputall, 96, 119));
            List<double> quan_zhong_xi_shu_list_4 = new List<double>();
            double temp_index_4 = S_ij[8] + S_ij[9];
            quan_zhong_xi_shu_list_4.Add(S_ij[8] / temp_index_4);
            quan_zhong_xi_shu_list_4.Add(S_ij[9] / temp_index_4);

            //Zhi_Shu_List.Clear();
            Ji_Xiao_List = new List<double>();
            listGroup = list_fen_zu1(index4, OBJECTNUM1);
            foreach (var item in listGroup)
            {
                Ji_Xiao_List.Add(Zhi_Shu(quan_zhong_xi_shu_list_4, item));
            }
            Console.WriteLine("\n");
            Console.WriteLine("{0:D}年小指数:绩效:", year);
            init_dictionary(out Zhi_Shu_Zi_Dian, Ji_Xiao_List);
            PrintData(Zhi_Shu_Zi_Dian, Ji_Xiao_List);
        }
        #endregion
        #region 初始化字典
        static void init_dictionary(out Dictionary<string, double> Zhi_Shu_Zi_Dian, List<double> Zhi_Shu_List)
        {
            Zhi_Shu_Zi_Dian = new Dictionary<string, double>();
            Zhi_Shu_Zi_Dian.Add("陕西", Zhi_Shu_List[0]);
            Zhi_Shu_Zi_Dian.Add("甘肃", Zhi_Shu_List[1]);
            Zhi_Shu_Zi_Dian.Add("宁夏", Zhi_Shu_List[2]);
            Zhi_Shu_Zi_Dian.Add("青海", Zhi_Shu_List[3]);
            Zhi_Shu_Zi_Dian.Add("新疆", Zhi_Shu_List[4]);
            Zhi_Shu_Zi_Dian.Add("广西", Zhi_Shu_List[5]);
            Zhi_Shu_Zi_Dian.Add("黑龙江", Zhi_Shu_List[6]);
            Zhi_Shu_Zi_Dian.Add("吉林", Zhi_Shu_List[7]);
            Zhi_Shu_Zi_Dian.Add("辽宁", Zhi_Shu_List[8]);
            Zhi_Shu_Zi_Dian.Add("内蒙", Zhi_Shu_List[9]);
            Zhi_Shu_Zi_Dian.Add("云南", Zhi_Shu_List[10]);
            Zhi_Shu_Zi_Dian.Add("重庆", Zhi_Shu_List[11]);
        }
        #endregion
        #region BP神经网络核心代码
        //初始化input和output Array
        static void init_input_and_output_data(double[][] input, double[][] output, List<List<double>> listGroup, List<double> Zhi_Shu_List,int num)
        {
            int i, k = 0;
            for (i = num * OBJECTNUM1, k = 0; i < (num + 1) * OBJECTNUM1; i++,k++)
            {
                input[i] = new double[IndexNUM];
                output[i] = new double[1];
                for (int j = 0; j < IndexNUM; j++)
                {
                    input[i][j] = listGroup[k][j];                 
                }
                output[i][0] = Zhi_Shu_List[k];
            }
        }
        static void SearchSolution(out BackPropagationLearning teacher,double[][] input, double[][] output)
        {
            ActivationNetwork network = new ActivationNetwork(
                new BipolarSigmoidFunction(2),
                Input_layer, Hidden_layer, Output_layer);
            teacher = new BackPropagationLearning(network);
            teacher.LearningRate = Learning_Rate;
            teacher.Momentum = Momentum;
            int iteration = 1;

            double error = 1.0;
            while (error > 0.001)
            {
                error = teacher.RunEpoch(input, output) / OBJECTNUM;
                Console.WriteLine(error);
                iteration++;
                if ((iterations != 0) && (iteration > iterations))
                    break;
            }
            Console.WriteLine("训练了{0:D}次", iteration);
        }
        #endregion
        #region 封装
        //输出数据
        static void PrintData(Dictionary<string, double> Zhi_Shu_Zi_Dian, List<double> Zhi_Shu_List)
        {
            foreach (KeyValuePair<string, double> kvp in Zhi_Shu_Zi_Dian)
            {
                Console.WriteLine("省份：{0},指数：{1}", kvp.Key, kvp.Value);
            }
            Console.WriteLine("");
            Console.WriteLine("根据上面计算出的指数,从小到大的排序如下:");
            Zhi_Shu_List.Sort();
            foreach (var item in Zhi_Shu_List)
            {
                foreach (KeyValuePair<string, double> kv in Zhi_Shu_Zi_Dian)
                {

                    if (item.Equals(kv.Value))
                    {
                        Console.WriteLine("{0}", kv.Key);
                    }
                }
            }
        }

        //计算出指数列表
        static List<double> Compute_ZhiShu_List(List<List<double>> listGroup, List<double> quan_zhong_xi_shu_list)
        {
            List<double> Zhi_Shu_List = new List<double>();
            foreach (var item in listGroup)
            {
                Zhi_Shu_List.Add(Zhi_Shu(quan_zhong_xi_shu_list, item));
            }
            return Zhi_Shu_List;
        }

        //计算出熵值列表
        static void Compute_Shang_List(List<List<double>> listGroup, out List<double> shang_list)
        {
            double temp = 0;
            shang_list = new List<double>();
            for (int i = 0; i < IndexNUM; i++)
            {
                temp = Te_Zheng_Bi_Zhong_and_Shang(listGroup[i]);
                shang_list.Add(temp);
            }
        }
        //把总的数据分割成2011的总数据或2012的总数据或2013的总数据或2014的总数据  AllData：总数据 k：（0代表2011，1代表2012，2代表2013，3代表2014）
        static List<double> Split_All_Year_Data_To_Eachyear(List<double> AllData, int k)
        {
            List<double> ReturnList = new List<double>();
            while (ReturnList.Count() < OBJECTNUM1 * IndexNUM)
            {
                ReturnList.Add(AllData[k]);
                k += 4;
            }
            return ReturnList;
        }

        //归一化数据
        static List<double> Normalization(List<List<double>> listGroup)
        {
            //  计算出正向指标，结果输出到 output
            List<double> outputall = new List<double>();
            List<double> output;
            for (int i = 0; i < listGroup.Count() - 1; i++)
            {
                Cal_Forward_Indicator(listGroup[i], out output);
                foreach (var item in output)
                {
                    //Console.WriteLine(item);
                    outputall.Add(item);
                }
            }

            //  计算出逆向指标，结果输出到 outputlist
            List<double> outputlist;
            Cal_Back_Indicator(listGroup[listGroup.Count() - 1], out outputlist);
            foreach (var item in outputlist)
            {
                //Console.WriteLine(item);
                outputall.Add(item);
            }
            return outputall;
        }
        #endregion
        #region 读数据
        static List<double> ReadFileByLine(string filePath)
        {
            string[] fileContents = File.ReadAllLines(filePath, Encoding.Default);
            List<double> fileList = new List<double>();
            foreach (string item in fileContents)
            {
                fileList.Add(Convert.ToDouble(item));
            }
            return fileList;
        }
        #endregion
        #region 指数

        static List<double> GetSubList(List<double> holelist, int fromIndex, int toIndex)
        {
            List<double> result = new List<double>();
            for (int i = fromIndex; i <= toIndex; i++)
            {
                result.Add(holelist[i]);
            }
            return result;
        }
        static List<List<double>> list_fen_zu(List<double> input, int zuNum,int objctnum)
        {
            List<List<double>> listGroup = new List<List<double>>();
            for (int i = 0; i < zuNum; i++)
            {
                List<double> cList = new List<double>();
                for (int j = 0 + i * objctnum; j < objctnum + i * objctnum; j++)
                {

                    cList.Add(input[j]);
                }
                listGroup.Add(cList);
            }
            return listGroup;
        }
        static List<List<double>> list_fen_zu1(List<double> input, int zuNum)
        {
            List<List<double>> listGroup = new List<List<double>>();
            int j = 0;
            for (int i = 0; i < zuNum; i++)
            {
                List<double> cList = new List<double>();
                j = i;
                while (j < input.Count())
                {
                    cList.Add(input[j]);
                    j += zuNum;
                }
                listGroup.Add(cList);
            }
            return listGroup;
        }

        //正向指标
        //input_index是所有省某一个指标4年的数据总合
        static void Cal_Forward_Indicator(List<double> input_index, out List<double> output_index)
        {
            output_index = new List<double>();

            foreach (var item in input_index)
            {
                output_index.Add((item - input_index.Min()) / (input_index.Max() - input_index.Min()));
            }
        }

        //逆向指标
        //input_index是所有省某一个指标4年的数据总合
        static void Cal_Back_Indicator(List<double> input_index, out List<double> output_index)
        {
            output_index = new List<double>();

            foreach (var item in input_index)
            {
                output_index.Add((input_index.Max() - item) / (input_index.Max() - input_index.Min()));
            }
        }

        //计算每个指标的熵值
        //input_index是所有省某一个指标某一年的数据总合
        //return  每个指标的熵值
        static double Te_Zheng_Bi_Zhong_and_Shang(List<double> input_index)
        {
            //特征比重
            //output_index = new List<double>();
            List<double> temp = new List<double>();
            double sum1 = 0;
            double sum2 = 0;
            double shang = 0;
            //double w = 0;
            foreach (var item in input_index)
            {
                sum1 += item;
            }
            foreach (var item in input_index)
            {
                temp.Add(item / sum1);
            }
            //熵值
            foreach (var item in temp)
            {
                if (item == 0)
                {
                    continue;
                }
                sum2 += item * System.Math.Log(item);
            }
            shang = sum2 * (-1 / System.Math.Log(OBJECTNUM));
            return shang;
        }

        //计算权重系数
        // input_index是输入的各个指标的熵值list (上述的shang list)
        //output_index是各个指标的权重系数
        static void Quan_Zhong_xi_shu(List<double> input_index, out List<double> output_index)
        {
            output_index = new List<double>();

            double sum = 0;
            foreach (var item in input_index)
            {
                sum += (1 - item);
            }

            foreach (var item in input_index)
            {
                output_index.Add((1 - item) / sum);
            }
        }

        //指数计算
        //input_quanzhong是权重系数list
        //input_index是某年某省的指标list
        static double Zhi_Shu(List<double> input_index, List<double> input_quanzhong)
        {
            double sum = 0;
            for (int i = 0; i < input_index.Count(); i++)
            {
                sum += input_index[i] * input_quanzhong[i];
            }
            return sum;
        }
        #endregion
        #region 蒙特卡罗
        static List<double> Chan_Sheng_Sui_Ji_Quan_Zhong(int num)
        {
            long tick = DateTime.Now.Ticks;
            Random ro = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            List<double> temp = new List<double>();
            List<double> test = new List<double>();
            double sum = 0;
            double result = 0;
            for (int i = 0; i < num; i++)
            {
                result = ro.Next();
                //Console.WriteLine(result);
                temp.Add(result);
                sum += result;
            }
            foreach (var item in temp)
            {
                test.Add(item / sum);
            }
            return test;
        }
        #endregion
        #region k-means算法
        // 初始化质心
        static void InitializeCentroids(List<Node> Yangben_arg, int numberOfClusters)
        {
            Random random = new Random(numberOfClusters);
            for (int i = 0; i < numberOfClusters; ++i)
            {
                Yangben_arg[i].Cluster = i;
            }
            for (int i = numberOfClusters; i < Yangben_arg.Count; i++)
            {
                Yangben_arg[i].Cluster  = random.Next(0, numberOfClusters);
            }
        }
        //更新均值（Means）
        static bool UpdateNodeMeans(List<Node> Yangben_arg, List<Node> clusters)
        {
            if (EmptyCluster(Yangben_arg)) return false;

            var groupToComputeMeans = Yangben_arg.GroupBy(s => s.Cluster).OrderBy(s => s.Key);
            int clusterIndex = 0;
            double jichuhuanjing = 0.0;
            double touru = 0.0;
            double chanchu = 0.0;
            double jixiao = 0.0;
            foreach (var item in groupToComputeMeans)
            {
                foreach (var value in item)
                {
                    jichuhuanjing += value.Ji_Chu_Huan_Jing;
                    touru += value.Tou_Ru;
                    chanchu += value.Chan_chu;
                    jixiao += value.Ji_xiao;
                }
                clusters[clusterIndex].Ji_Chu_Huan_Jing = jichuhuanjing / item.Count();
                clusters[clusterIndex].Tou_Ru = touru / item.Count();
                clusters[clusterIndex].Chan_chu = chanchu / item.Count();
                clusters[clusterIndex].Ji_xiao = jixiao / item.Count();
                clusterIndex++;
                jichuhuanjing = 0.0;
                touru = 0.0;
                chanchu = 0.0;
                jixiao = 0.0;
            }
            return true;
        }
        //测试组内是否为空
        static bool EmptyCluster(List<Node> data)
        {
            var emptyCluster =
                data.GroupBy(s => s.Cluster).OrderBy(s => s.Key).Select(g => new { Cluster = g.Key, Count = g.Count() });

            foreach (var item in emptyCluster)
            {
                if (item.Count == 0)
                {
                    return true;
                }
            }
            return false;
        }
        //更新组内成员
        static bool UpdateClusterMembership(int _numberOfClusters, List<Node> _normalizedDataToCluster, List<Node> _clusters)
        {
            bool changed = false;

            double[] distances = new double[_numberOfClusters];

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _normalizedDataToCluster.Count; ++i)
            {

                for (int k = 0; k < _numberOfClusters; ++k)
                    distances[k] = ElucidanDistance(_normalizedDataToCluster[i], _clusters[k]);

                int newClusterId = MinIndex(distances);
                if (newClusterId != _normalizedDataToCluster[i].Cluster)
                {
                    changed = true;
                    _normalizedDataToCluster[i].Cluster = newClusterId;
                }
            }
            if (changed == false)
                return false;
            if (EmptyCluster(_normalizedDataToCluster)) return false;
            return true;
        }
        //计算欧式距离
        static double ElucidanDistance(Node node, Node mean)
        {
            double _diffs = 0.0;
            _diffs = Math.Pow(node.Ji_Chu_Huan_Jing - mean.Ji_Chu_Huan_Jing, 2);
            _diffs += Math.Pow(node.Tou_Ru - mean.Tou_Ru, 2);
            _diffs += Math.Pow(node.Chan_chu - mean.Chan_chu, 2);
            _diffs += Math.Pow(node.Ji_xiao - mean.Ji_xiao, 2);
            return Math.Sqrt(_diffs);
        }

        //计算出距离某一组欧式距离近 的下标
        static int MinIndex(double[] distances)
        {
            int _indexOfMin = 0;
            double _smallDist = distances[0];
            for (int k = 0; k < distances.Length; ++k)
            {
                if (distances[k] < _smallDist)
                {
                    _smallDist = distances[k];
                    _indexOfMin = k;
                }
            }
            return _indexOfMin;
        }
        #endregion
        #region 神经网络计算权重的一些函数
        public static double[][] Rotate(double[][] array)
        {
            double[][] newArray = new double[array[0].Length][]; //构造转置二维数组
            for (int i = 0; i < array[0].Length; i++)
            {
                newArray[i] = new double[array.Length];
            }
            for (int i = 0; i < newArray.Length; i++)
            {
                for (int j = 0; j < newArray[0].Length; j++)
                {
                    newArray[i][j] = array[j][i];
                }
            }
            return newArray;
        }
        static void Compute_r_ij(out List<double>r_ij,double[][] W_ki,double[][] W_jk)
        {
            double[][] newW_ki = Rotate(W_ki);
            r_ij = new List<double>();
            for (int i = 0; i < Input_layer; i++)
            {
                double item = 0;
                for (int k = 0; k < Hidden_layer; k++)
                {
                    item += newW_ki[i][k] * (1 - Math.Exp(-W_jk[0][k])) / (1+ Math.Exp(-W_jk[0][k]));
                }
                r_ij.Add(item);
            }
        }
        static void Compute_R_ij(out List<double> R_ij, List<double> r_ij)
        {
            R_ij = new List<double>();
            for (int i = 0; i < Input_layer; i++)
            {
                double item = 0;
                item = Math.Abs((1-Math.Exp(-r_ij[i]))/(1 + Math.Exp(-r_ij[i])));
                R_ij.Add(item);
            }
        }
        static void Compute_S_ij(out List<double> S_ij, List<double> R_ij)
        {
            S_ij = new List<double>();
            double sum = 0;
            for (int i = 0; i < Input_layer; i++)
            {
                sum += R_ij[i];
            }
            for (int j = 0; j < Input_layer; j++)
            {
                S_ij.Add(R_ij[j] / sum);
            }
        }
        #endregion
    }
}
