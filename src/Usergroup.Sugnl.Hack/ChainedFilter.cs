/**
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */



/**
 * <p>
 * Allows multiple {@link Filter}s to be chained.
 * Logical operations such as <b>NOT</b> and <b>XOR</b>
 * are applied between filters. One operation can be used
 * for all filters, or a specific operation can be declared
 * for each filter.
 * </p>
 * <p>
 * Order in which filters are called depends on
 * the position of the filter in the chain. It's probably
 * more efficient to place the most restrictive filters
 * /least computationally-intensive filters first.
 * </p>
 *
 */


/**
 * Kern - Please note that this code is taken from:
 * http://lucenenet.apache.org/docs/3.0.3/d6/d7c/_chained_filter_8cs_source.html
 */

namespace Lucene.Net.Analysis
{
    using System;
    using System.Text;
    using Lucene.Net.Index;
    using Lucene.Net.Search;
    using Lucene.Net.Util;

    public class ChainedFilter : Filter
    {
        public const int OR = 0;
        public const int AND = 1;
        public const int ANDNOT = 2;
        public const int XOR = 3;
        /**
         * Logical operation when none is declared. Defaults to
         * OR.
         */
        public static int DEFAULT = OR;

        /** The filter chain */
        private Filter[] chain = null;

        private int[] logicArray;

        private int logic = -1;

        /**
         * Ctor.
         * @param chain The chain of filters
         */
        public ChainedFilter(Filter[] chain)
        {
            this.chain = chain;
        }

        /**
         * Ctor.
         * @param chain The chain of filters
         * @param logicArray Logical operations to apply between filters
         */
        public ChainedFilter(Filter[] chain, int[] logicArray)
        {
            this.chain = chain;
            this.logicArray = logicArray;
        }

        /**
         * Ctor.
         * @param chain The chain of filters
         * @param logic Logical operation to apply to ALL filters
         */
        public ChainedFilter(Filter[] chain, int logic)
        {
            this.chain = chain;
            this.logic = logic;
        }

        /**
         * {@link Filter#GetDocIdSet}.
         */
        public override DocIdSet GetDocIdSet(IndexReader reader)
        {
            int[] index = new int[1]; // use array as reference to modifiable int; 
            index[0] = 0;             // an object attribute would not be thread safe.
            if (logic != -1)
                return GetDocIdSet(reader, logic, index);
            else if (logicArray != null)
                return GetDocIdSet(reader, logicArray, index);
            else
                return GetDocIdSet(reader, DEFAULT, index);
        }

        private DocIdSetIterator getDISI(Filter filter, IndexReader reader)
        {
            DocIdSet docIdSet = filter.GetDocIdSet(reader);
            if (docIdSet == null)
            {
                return DocIdSet.EMPTY_DOCIDSET.Iterator();
            }
            else
            {
                DocIdSetIterator iter = docIdSet.Iterator();
                if (iter == null)
                {
                    return DocIdSet.EMPTY_DOCIDSET.Iterator();
                }
                else
                {
                    return iter;
                }
            }
        }

        private OpenBitSetDISI initialResult(IndexReader reader, int logic, int[] index)
        {
            OpenBitSetDISI result;
            /**
             * First AND operation takes place against a completely false
             * bitset and will always return zero results.
             */
            if (logic == AND)
            {
                result = new OpenBitSetDISI(getDISI(chain[index[0]], reader), reader.MaxDoc);
                ++index[0];
            }
            else if (logic == ANDNOT)
            {
                result = new OpenBitSetDISI(getDISI(chain[index[0]], reader), reader.MaxDoc);
                result.Flip(0, reader.MaxDoc); // NOTE: may set bits for deleted docs.
                ++index[0];
            }
            else
            {
                result = new OpenBitSetDISI(reader.MaxDoc);
            }
            return result;
        }

        /** Provide a SortedVIntList when it is definitely
         *  smaller than an OpenBitSet
         *  @deprecated Either use CachingWrapperFilter, or
         *  switch to a different DocIdSet implementation yourself.
         *  This method will be removed in Lucene 4.0 
         **/
        protected DocIdSet finalResult(OpenBitSetDISI result, int maxDocs)
        {
            return result;
        }


        /**
         * Delegates to each filter in the chain.
         * @param reader IndexReader
         * @param logic Logical operation
         * @return DocIdSet
         */
        private DocIdSet GetDocIdSet(IndexReader reader, int logic, int[] index)
        {
            OpenBitSetDISI result = initialResult(reader, logic, index);
            for (; index[0] < chain.Length; index[0]++)
            {
                doChain(result, logic, chain[index[0]].GetDocIdSet(reader));
            }
            return finalResult(result, reader.MaxDoc);
        }

        /**
         * Delegates to each filter in the chain.
         * @param reader IndexReader
         * @param logic Logical operation
         * @return DocIdSet
         */
        private DocIdSet GetDocIdSet(IndexReader reader, int[] logic, int[] index)
        {
            if (logic.Length != chain.Length)
                throw new ArgumentException("Invalid number of elements in logic array");

            OpenBitSetDISI result = initialResult(reader, logic[0], index);
            for (; index[0] < chain.Length; index[0]++)
            {
                doChain(result, logic[index[0]], chain[index[0]].GetDocIdSet(reader));
            }
            return finalResult(result, reader.MaxDoc);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("ChainedFilter: [");
            for (int i = 0; i < chain.Length; i++)
            {
                sb.Append(chain[i]);
                sb.Append(' ');
            }
            sb.Append(']');
            return sb.ToString();
        }

        private void doChain(OpenBitSetDISI result, int logic, DocIdSet dis)
        {

            if (dis is OpenBitSet)
            {
                // optimized case for OpenBitSets
                switch (logic)
                {
                    case OR:
                        result.Or((OpenBitSet)dis);
                        break;
                    case AND:
                        result.And((OpenBitSet)dis);
                        break;
                    case ANDNOT:
                        result.AndNot((OpenBitSet)dis);
                        break;
                    case XOR:
                        result.Xor((OpenBitSet)dis);
                        break;
                    default:
                        doChain(result, DEFAULT, dis);
                        break;
                }
            }
            else
            {
                DocIdSetIterator disi;
                if (dis == null)
                {
                    disi = DocIdSet.EMPTY_DOCIDSET.Iterator();
                }
                else
                {
                    disi = dis.Iterator();
                    if (disi == null)
                    {
                        disi = DocIdSet.EMPTY_DOCIDSET.Iterator();
                    }
                }

                switch (logic)
                {
                    case OR:
                        result.InPlaceOr(disi);
                        break;
                    case AND:
                        result.InPlaceAnd(disi);
                        break;
                    case ANDNOT:
                        result.InPlaceNot(disi);
                        break;
                    case XOR:
                        result.InPlaceXor(disi);
                        break;
                    default:
                        doChain(result, DEFAULT, dis);
                        break;
                }
            }
        }

    }
}