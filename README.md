# MPL.Bitcoin
A collection of utility libraries and tools for Bitcoin written in C#.

## MPL.Bitcoin.BlockchainParser

A library that provides parsers for the Bitcoin blockchain.

Each parser can be supplied with a `byte array` or a `Stream` in order to parse data.  In addition, the `BlockFileParser` can also read raw `blk?????.dat` files and parse all blocks within them.
The `Stream` parser can work with live streams from online sources in the case of a data feed from a Bitcoin node.

## MPL.Bitcoin.Library

Contains a standard library of common Bitcoin-related objects, as well as generic helper functions common across many Bitcoin-related tools and libraries.

## Notes

This is a work-in-progress that I will be adding to when I have time to integrate some of my other Bitcoin-related projects.

The following are currently written but not yet integrated:

- Key generator that generates uncompressed and compressed legacy (`1x`) addresses, P2MS (`m-`) addresses, P2SH (P2PKH and multisig) (`3x`) addresses, and Bech32 (`bc1`) addresses.
- Bulk key generator that can follow defined rules to generate random or sequential keys.
- Key converter that converts keys betweem Bitcoin and other related coins (BCH, Doge, Litecoin, etc.).
- Dictionary-based address generator.
- Transaction signing and ECDSA message digest (`z`) calculator.
- ECDSA signature exploit resulting in private key reversal based on weakness when using repeated `r` values.
- ECDSA multiplier calculator for private key reversal in certain scenarios.
- Bitcoin node monitor which provides alerting and notification on defined rulesets.
- More shared tools including Base32 converter, Base58 converter, and key manipulation utilities.
