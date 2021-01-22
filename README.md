# MPL.Bitcoin
A collection of utility libraries and tools for Bitcoin written in C#.

## MPL.Bitcoin.BlockchainParser

A library that provides parsers for the Bitcoin blockchain.

Each parser can be supplied with a `byte array` or a `Stream` in order to parse data.  In addition, the `BlockFileParser` can also read raw `blk?????.dat` files and parse all blocks within them.
The `Stream` parser can work with live streams from online sources in the case of a data feed from a Bitcoin node.

## MPL.Bitcoin.Library

Contains a standard library of common Bitcoin-related objects, as well as generic helper functions common across many Bitcoin-related tools and libraries.

