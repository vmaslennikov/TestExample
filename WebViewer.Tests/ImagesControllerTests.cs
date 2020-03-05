using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebViewer.Controllers;
using WebViewer.Models;
using WebViewer.Services;

namespace WebViewer.Tests {
    public class ImagesControllerTests {
        private ImagesController controller;
        Mock<IImageService> _imageservice;
        Mock<ILogger<ImagesController>> _logger;

        [SetUp]
        public void Setup () {
            _imageservice = new Mock<IImageService> ();
            _imageservice.Setup (s => s.GetFolders (null)).Returns (new List<Folder> {
                new Folder {
                    Root = "/wwwroot/images/autumn",
                        Name = "autumn",
                        Path = "C:\\src\\github\\WebViewer\\wwwroot\\images\\autumn",
                        Children = new List<Folder> () {
                            new Folder {
                                Root = "/wwwroot/images/autumn/sub_01",
                                    Name = "sub_02",
                                    Path = "C:\\src\\github\\WebViewer\\wwwroot\\images\\autumn\\sub_01",
                                    Children = new List<Folder> ()
                            },
                            new Folder {
                                Root = "/wwwroot/images/autumn/sub_02",
                                    Name = "sub_02",
                                    Path = "C:\\src\\github\\WebViewer\\wwwroot\\images\\autumn\\sub_02",
                                    Children = new List<Folder> ()
                            }
                        }
                }
            });
            _imageservice.Setup (s => s.GetFolders ("autumn")).Returns (new List<Folder> {
                new Folder {
                    Root = "/wwwroot/images/autumn/sub_01",
                        Name = "sub_02",
                        Path = "C:\\src\\github\\WebViewer\\wwwroot\\images\\autumn\\sub_01",
                        Children = new List<Folder> ()
                },
                new Folder {
                    Root = "/wwwroot/images/autumn/sub_02",
                        Name = "sub_02",
                        Path = "C:\\src\\github\\WebViewer\\wwwroot\\images\\autumn\\sub_02",
                        Children = new List<Folder> ()
                }
            });
        }

        [Test]
        public void GetRootFolder () {
            Mock<IImageService> _imageservice = new Mock<IImageService> ();
            Mock<ILogger<ImagesController>> _logger = new Mock<ILogger<ImagesController>> ();
            var data = new Folder {
                Root = "/wwwroot/images/autumn",
                Name = "autumn",
                Path = "C:\\src\\github\\WebViewer\\wwwroot\\images\\autumn",
                Children = new List<Folder> () {
                new Folder {
                Root = "/wwwroot/images/autumn/sub_01",
                Name = "sub_02",
                Path = "C:\\src\\github\\WebViewer\\wwwroot\\images\\autumn\\sub_01",
                Children = new List<Folder> ()
                },
                new Folder {
                Root = "/wwwroot/images/autumn/sub_02",
                Name = "sub_02",
                Path = "C:\\src\\github\\WebViewer\\wwwroot\\images\\autumn\\sub_02",
                Children = new List<Folder> ()
                }
                }
            };
            _imageservice.Setup (s => s.GetFolders (null)).Returns (new List<Folder> { data });

            ImagesController controller = new ImagesController (_imageservice.Object, _logger.Object);
            // act 
            IActionResult cResult = controller.GetFolders (null);
            var okResult = cResult as OkObjectResult;
            // assert
            Assert.IsNotNull (okResult);
            Assert.AreEqual (200, okResult.StatusCode);
            Assert.IsInstanceOf<List<Folder>> (okResult.Value);
        }

        [Test]
        public void GetChildFolder () {
            Mock<IImageService> _imageservice = new Mock<IImageService> ();
            Mock<ILogger<ImagesController>> _logger = new Mock<ILogger<ImagesController>> ();
            var data = new List<Folder> () {
                new Folder {
                Root = "/wwwroot/images/autumn/sub_01",
                Name = "sub_02",
                Path = "C:\\src\\github\\WebViewer\\wwwroot\\images\\autumn\\sub_01",
                Children = new List<Folder> ()
                },
                new Folder {
                Root = "/wwwroot/images/autumn/sub_02",
                Name = "sub_02",
                Path = "C:\\src\\github\\WebViewer\\wwwroot\\images\\autumn\\sub_02",
                Children = new List<Folder> ()
                }
            };
            _imageservice.Setup (s => s.GetFolders ("autumn")).Returns (data);

            ImagesController controller = new ImagesController (_imageservice.Object, _logger.Object);
            // act 
            IActionResult cResult = controller.GetFolders ("autumn");
            var okResult = cResult as OkObjectResult;
            // assert
            Assert.IsNotNull (okResult);
            Assert.AreEqual (200, okResult.StatusCode);
            Assert.IsInstanceOf<List<Folder>> (okResult.Value);
        }

        [Test]
        public void GetRootFiles () {
            Mock<IImageService> _imageservice = new Mock<IImageService> ();
            Mock<ILogger<ImagesController>> _logger = new Mock<ILogger<ImagesController>> ();
            var emptyData = new List<ImageFile> { };
            _imageservice.Setup (s => s.GetImages ("root")).Returns (emptyData);
            ImagesController controller = new ImagesController (_imageservice.Object, _logger.Object);
            // act 
            IActionResult cResult = controller.GetFiles ("root");
            var okResult = cResult as OkObjectResult;
            // assert
            Assert.IsNotNull (okResult);
            Assert.AreEqual (200, okResult.StatusCode);
            Assert.IsInstanceOf<List<ImageFile>> (okResult.Value);
        }

        [Test]
        public void GetFiles () {
            Mock<IImageService> _imageservice = new Mock<IImageService> ();
            Mock<ILogger<ImagesController>> _logger = new Mock<ILogger<ImagesController>> ();
            var data = new List<ImageFile> {
                new ImageFile {
                Name = "filename",
                }
            };
            _imageservice.Setup (s => s.GetImages ("autumn")).Returns (data);
            ImagesController controller = new ImagesController (_imageservice.Object, _logger.Object);
            // act 
            IActionResult cResult = controller.GetFiles ("autumn");
            var okResult = cResult as OkObjectResult;
            // assert
            Assert.IsNotNull (okResult);
            Assert.AreEqual (200, okResult.StatusCode);
            Assert.IsInstanceOf<List<ImageFile>> (okResult.Value);
        }
    }
}