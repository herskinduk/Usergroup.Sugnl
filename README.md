# Usergroup.Sugnl

Example code for my @SUG_NL SUGCON [presentation](http://herskinduk.github.io/Usergroup.Sugnl/#/begin) on Sitecore Search.

## Examples

### Multi-component search page example

- Search Service with per request caching decorator
- Criteria class with custom GetHashCode method
- Pivot Facet result parsing

### Geo-spatial hack for Sitecore Lucene

- Generate Geohash strings 
- Within circle area filter
- Otional acendinhg/descending distance scoring (not a 'sort' in the pure sense - more like rescoring)

## TODO

- [ ] Incorporate [unit test example](https://github.com/Sitecore/sitecore-seven-unittest-example/blob/master/ExampleFixture.cs) 
- [ ] Make spatial filter apply to Facet results
- [ ] Cleanup/rationalise interfaces and models

## Get going

This solution examples where tested against a Sitecore 7.2. To work with the solution locally create a "lib" folder in the repository root with the Sitecore binaries and refetch the nuget dependencies.

To run the example locally you need to deploy the code and create definition ites for the controller renderings (for the SearchController).

The code and presentation uses the following projects:

- Lucene.Net.Contrib.Spatial (via Nuget)
- The presentation is based on [impress.js](https://github.com/bartaz/impress.js) and [highlight.js](https://github.com/isagalaev/highlight.js)
